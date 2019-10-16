import React, { Component } from 'react';
import {Route, Switch} from 'react-router-dom';

import Home from '../Home/Home';
import Profile from '../Profile/Profile';
import Login from '../Authorization/Login'

export default class Routes extends Component {
  render() {
    return (
      <div>
          <Switch>
            <Route exact path='/' component={Home} />
            <Route path='/profile' component={Profile} />
            <Route path='/login' component={Login} />
          </Switch>
      </div>
    );
  }
}
