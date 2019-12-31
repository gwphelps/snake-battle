
class IndexPage extends React.Component {
    constructor(props) {
        super(props);
        this.state = { username: "", userType:"host" };
        this.handleChange = this.handleChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.setUserType = this.setUserType.bind(this);
    }

    handleChange(event) {
        this.setState({ username: event.target.value });
    }


    handleSubmit() {
        
        const xhr = new XMLHttpRequest();

        if (this.state.userType == "host") {
            
            xhr.open('post', "/hostgame", true);
            const data = new FormData();
            data.append('username', this.state.username);
            xhr.onload = () => window.location.href = "/hostpage?username=" + this.state.username;
            
            xhr.send(data);
        }
        else if (this.state.userType == "join") {
            
            window.location.href = "/joinpage?username=" + this.state.username;
           
        }
    }

    setUserType(event){
        this.setState({ userType: event.target.value });
    }

    render() {
        return(
            <div>
                <h2>Enter a temporary username</h2>
                <form>
                    <input type="text" placeholder="username" onChange={this.handleChange} />
                    <select value={this.state.userType} onChange={this.setUserType}>
                        <option value="join">Join</option>
                        <option value="host">Host</option>
                    </select>
                    <input type="button" value="submit" onClick={ () => this.handleSubmit()} />
                </form>
            </div>
        )
    }
}

ReactDOM.render(<IndexPage pollInterval={250} />, document.getElementById('root'));