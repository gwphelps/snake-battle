class HostPage extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            guest: "",
            phrase:""
        };
    }

    componentDidMount() {
        
        window.setInterval(
            () => {
                
                this.checkStatus();
            },
            1000
        );
    }

    checkStatus() {
        const xhr = new XMLHttpRequest();
        xhr.open("get", "/checkstatus?username=" + this.props.username, true);
        xhr.onload = () => {
            const response = JSON.parse(xhr.response);
            
            
            if (response != "") {
                

                this.setState({
                    guest: response,
                    phrase: 
                        <div>
                            <div>{response} wants to join!</div>
                            <input type="button" value="start" onClick={ () => this.startGame()}/>
                        </div>
                     
                });
                
            }
        };
        xhr.send();
    }

    startGame() {
        window.location.href = "/game?username=" + this.props.username;
        
    }
    render() {
        return (
            <div>
                <h2>Hey {this.props.username}!</h2>
                <div>Waiting for someone to join...</div>
                <div>{this.state.phrase}</div>
            </div>
        ); 
    }
}
ReactDOM.render(<HostPage username={mUsername}/>, document.getElementById("root"));