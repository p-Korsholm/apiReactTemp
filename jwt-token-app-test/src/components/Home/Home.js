import React, { Component } from "react";
import { connect } from "react-redux";
import { getHello } from "../../redux/Actions/Hello";
class Home extends Component {
  componentDidMount() {
    this.props.getHello();
  }

  render() {
    return (
      <div>
        <h3> Home </h3>
        {this.props.isLoading ? (
          <p>loading...</p>
        ) : this.props.hasErrored ? (
          this.props.errorMessages.map((v, k) => <p key={k}>{v}</p>)
        ) : (
          this.props.data
        )}
      </div>
    );
  }
}

Home.propTypes = {};
Home.defaultProps = {};

const mapStateToProps = state => {
  return {
    data: state.Hello.data,
    hasErrored: state.Hello.hasErrored,
    isLoading: state.Hello.isLoading,
    errorMessages: state.Hello.errorMessages,
  };
};

export default connect(
  mapStateToProps,
  { getHello }
)(Home);
