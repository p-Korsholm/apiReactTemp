import React, { Component } from "react";
import { Route, Switch } from "react-router-dom";

import Home from "../Home/Home";
import Profile from "../Profile/Profile";
import Login from "../Authorization/Login";

export default class Routes extends Component {
  render() {
    return (
      <div>
        <Switch>
          <Route exact path="/" render={props => <Home {...props} />} />
          <Route path="/profile" render={props => <Profile {...props} />} />
          <Route path="/login" render={props => <Login {...props} />} />
        </Switch>
      </div>
    );
  }
}
