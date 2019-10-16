import React, { Component } from "react";
import { connect } from "react-redux";
import { login, logout } from "../../redux/Actions/Authorization";

class Login extends Component {
  constructor(props) {
    super(props);

    this.state = { username: "", password: "", message: [] };

    this.handleInputChanged = this.handleInputChanged.bind(this);
    this.handleLogin = this.handleLogin.bind(this);
  }

  handleInputChanged(event) {
    this.setState({
      [event.target.name]: event.target.value,
    });
  }

  handleLogin(event) {
    const loginModel = { ...this.state };
    this.props.login(loginModel);
  }

  render() {
    return (
      <div>
        <div>
          {this.props.isLoading ? (
            <p>loading...</p>
          ) : (
            this.props.errorMessages.map((s, i) => <p key={i}>{s}</p>)
          )}
        </div>
        <form>
          <table>
            <tbody>
              <tr>
                <td>username</td>
                <td>
                  <input name="username" onChange={this.handleInputChanged} />
                </td>
              </tr>
              <tr>
                <td>password</td>
                <td>
                  <input
                    name="password"
                    type="password"
                    onChange={this.handleInputChanged}
                  />
                </td>
              </tr>
              <tr>
                <td></td>
                <td>
                  <input
                    type="button"
                    value="Submit"
                    onClick={this.handleLogin}
                  />
                </td>
              </tr>
            </tbody>
          </table>
          <input type="button" value="logout" onClick={this.props.logout} />
        </form>
      </div>
    );
  }
}

Login.propTypes = {};
Login.defaultProps = {};

const mapStateToProps = state => {
  return {
    data: state.Login.data,
    hasErrored: state.Login.hasErrored,
    isLoading: state.Login.isLoading,
    errorMessages: state.Login.errorMessages,
  };
};

export default connect(
  mapStateToProps,
  { login, logout }
)(Login);
