class JoinGame extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            hosts: [],

        };
        this.createList = this.createList.bind(this);
        this.joinGame = this.joinGame.bind(this);
    }

    loadHosts() {
        
        const xhr = new XMLHttpRequest();
        xhr.open("get", "/gethosts", true);
        xhr.onload = () => {
            let obj = JSON.parse(xhr.responseText);
            let hostArr = [];
            for (let i = 0; i < obj.length; i++) {
                hostArr.push(obj[i].Host);
            }
            
            //console.log(hostArr);
            this.setState({ hosts: hostArr });
        }
        xhr.send();
        
    }
    joinGame(host, member) {
        const xhr = new XMLHttpRequest();
        const data = new FormData();
        data.append("Host", host);
        data.append("Member", member);
        xhr.open("post", "/joinhost", true);
        xhr.onload = () => {
            
            window.setInterval(
                () => {
                    const xhr2 = new XMLHttpRequest();
                    
                    xhr2.open("get", "/checkStart?username=" + this.props.username);
                    xhr2.onload = () => {
                        if (JSON.parse(xhr2.response) == "true") {
                            
                            window.location.href = "/game?username=" + this.props.username;
                        }

                    }
                    xhr2.send();
                },
                    250
            );
            
        }
        xhr.send(data);
    }

    createList() {
        const arr = this.state.hosts;
        if (arr.length == 0) {
            return "No Games Available";
        }
        return arr.map((host) =>
            <li key={host}>
                {host}  <span><input type="button" value="Join" onClick={() => this.joinGame(host, this.props.username)}/></span>
            </li>
            
        );
    }

    componentDidMount() {
        this.loadHosts();
    }

    render() {
        return (
            <div>
                <h2>Hey {this.props.username}!</h2>
                <ul>{this.createList()}</ul>
            </div>
            )
    }
}


ReactDOM.render(<JoinGame username={mUsername}/>, document.getElementById('root'));