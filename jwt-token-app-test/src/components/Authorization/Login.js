import React, { Component } from 'react';
import axios from "axios";

export default class Login extends Component {
    constructor(props){
        super(props);

        this.state =    {username:"",
                         password:"", 
                         message:[]};

        this.handleInputChanged = this.handleInputChanged.bind(this);
        this.handleLogin = this.handleLogin.bind(this);
    }

    handleInputChanged(event){
      this.setState({
        [event.target.name]: event.target.value
      });
    }

    handleLogin(event){
      const loginModel = {...this.state};
      axios.post("http://localhost:5000/api/auth/token",loginModel).then(response =>{
        console.log(response.data)
        this.setState({message:[]});
        this.loadValues(response.data["token"]);
      }).catch((error) => {
        var s = [];
        console.log(error);
        for(var err in error.response.data)
          [...error.response.data[err]].forEach((v,i) => s.push(v))

        this.setState({message:s});
      });
    }

    loadValues(token){
      console.log(token);
      axios.get("http://localhost:5000/api/values",{ headers:{"Authorization":`bearer ${token}`}})
        .then(response => {
          this.setState({message:response.data});
        }).catch(r => {
          console.log(r);
        })
    }

    render() {
        return (
            <div>
              <div>{this.state.message.map((v,i) => <p key={i}>{v}</p>)}</div>
                <form>
                    <table>
                      <tbody>
                          <tr>
                              <td>username</td>
                              <td><input name="username" onChange={this.handleInputChanged}/></td>
                          </tr>
                          <tr>
                              <td>password</td>
                              <td><input name="password" type="password" onChange={this.handleInputChanged}/></td>
                          </tr>
                          <tr>
                              <td></td>
                              <td><input type="button" value="Submit" title="login" onClick={this.handleLogin}/></td>
                          </tr>
                      </tbody>
                    </table>
                </form>
            </div>
        );
    }
}
