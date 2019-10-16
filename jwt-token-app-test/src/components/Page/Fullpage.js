import React, { Component } from 'react';
import Header from './Header';
import Footer from './Footer';
import Nav from './Nav';
import Routes from './Routes';

export default class FullPage extends Component {
  render() {
    return (
        <div> 
            <Header />
            <Nav /> 
            <Routes />
            <Footer />
        </div>
    );
  }
}
