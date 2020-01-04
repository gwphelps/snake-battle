
class Canvas extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            hostSnake: {},
            memberSnake: {}, 
            fruit: {},
            hostSize: 0,
            hostBody: {},
            memSize: 0,
            memBody: {},
            hostName: "",
            memName: "",
            memWalls: {},
            hostWalls: {},
            direction: "d",
            drop: false,
            lost: "no"
        };
        this.sendBoard = this.sendBoard.bind(this);
        
    }
    drawBoard() {
        
        const canvas = this.refs.canvas;
        const ctx = canvas.getContext("2d");

        ctx.clearRect(0, 0, 500, 500);

        const fruitX = this.state.fruit.x;
        const fruitY = this.state.fruit.y;

        ctx.fillStyle = "red";
        ctx.fillRect(20 * fruitX, 20 * fruitY, 20, 20);

        const memX = this.state.memberSnake.x;
        const memY = this.state.memberSnake.y;

        ctx.fillStyle = "green";
        ctx.fillRect(20 * memX, 20 * memY, 20, 20);

        var memBodyArr = this.state.memBody;
        for (var i = 0; i < memBodyArr.length; i++) {
            ctx.fillRect(20 * memBodyArr[i].X, 20 * memBodyArr[i].Y, 20, 20);
        }

        ctx.fillStyle = "#70cc70";
        var memWallsArr = this.state.memWalls;
        for (var i = 0; i < memWallsArr.length; i++) {
            ctx.fillRect(20 * memWallsArr[i].X, 20 * memWallsArr[i].Y, 20, 20);
        }

        
        ctx.fillStyle = "black";
        ctx.fillRect(20 * memX + 2, 20 * memY + 8, 8, 4);
        ctx.fillRect(20 * memX + 14, 20 * memY + 8, 4, 4);

        const hostX = this.state.hostSnake.x;
        const hostY = this.state.hostSnake.y;

        ctx.fillStyle = "blue";
        ctx.fillRect(20 * hostX, 20 * hostY, 20, 20);

        var hostBodyArr = this.state.hostBody;
        for (var i = 0; i < hostBodyArr.length; i++) {
            ctx.fillRect(20 * hostBodyArr[i].X, 20 * hostBodyArr[i].Y, 20, 20);
        }

        ctx.fillStyle = "#7070cc";
        var hostWallsArr = this.state.hostWalls;
        for (var i = 0; i < hostWallsArr.length; i++) {
            ctx.fillRect(20 * hostWallsArr[i].X, 20 * hostWallsArr[i].Y, 20, 20);
        }

        ctx.fillStyle = "black";
        ctx.fillRect(20 * hostX + 2, 20 * hostY + 8, 4, 4);
        ctx.fillRect(20 * hostX + 14, 20 * hostY + 8, 4, 4);

        ctx.fillText(this.state.hostName + " : " + this.state.hostSize, 150, 450);
        ctx.fillText(this.state.memName + " : " + this.state.memSize, 350, 450);

        if (this.state.lost != "no") {
            ctx.fillStyle = "black";
            ctx.fillRect(20 * 8, 20 * 8, 12*13, 12*13);
            ctx.fillStyle = "white";
            ctx.font = "32px Arial";
            ctx.fillText(this.state.lost, 20 * 10, 20 * 11);
            ctx.fillText("Won!", 20 * 10, 22 * 12);
            
        }
    }

    getBoard() {
        
        const xhr = new XMLHttpRequest();
        xhr.open('get', "/getboard?username=" + this.props.username, true);
        xhr.onload = () => {
            const data = JSON.parse(xhr.responseText);
            
            this.setState({
                hostSnake: { x: data.HostSnake.X, y: data.HostSnake.Y },
                memberSnake: { x: data.MemberSnake.X, y: data.MemberSnake.Y },
                fruit: { x: data.Fruit.X, y: data.Fruit.Y },
                memSize: data.MemSize,
                hostSize: data.HostSize,
                hostName: data.Host,
                memName: data.Member,
                memBody: data.MemBody,
                hostBody: data.HostBody,
                memWalls: data.MemWalls,
                hostWalls: data.HostWalls,
                drop: false,
                lost: data.lost
            });
         
            //console.log(this.state);
           
            this.drawBoard();
        };
        xhr.send();
        
    }

    sendBoard() {
        const data = new FormData();
        data.append('key', this.state.direction);
        data.append("drop", this.state.drop);
        data.append('username', this.props.username);
        
        const xhr = new XMLHttpRequest();
        xhr.open('post', "/keypush", true);
        xhr.onload = () => {
            this.getBoard();
        }
        xhr.send(data);
    }

    componentDidMount() {

        this.drawBoard();

        window.setInterval(
            () => {
                if (this.state.lost == "no") {
                    this.sendBoard();
                }
                
            },
            this.props.pollInterval,
        );
    }
    render() {
        return (
            <div tabIndex="0" onKeyDown={(e) => {
                if (e.key == "c") {
                    this.state.drop = true;
                }
                else {
                    this.state.direction = e.key;
                }
                
            }}>
                <canvas ref="canvas" width="500" height="500" style={{ border: '1px solid #000000' }} />
            </div>
        )
    }
}

ReactDOM.render(<Canvas pollInterval={250} username={mUsername}/>, document.getElementById('root'));