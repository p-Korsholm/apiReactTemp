import axios from "axios";
import {
  LOGIN,
  LOGIN_SUCCESS,
  LOGIN_FAIL,
  LOGOUT,
} from "../Actions/Authorization";

export function Login(
  state = {
    data: [],
    isLoading: false,
    hasErrored: false,
    errorMessages: [],
  },
  action
) {
  switch (action.type) {
    case LOGIN: {
      return Object.assign({}, state, {
        isLoading: true,
        hasErrored: false,
      });
    }
    case LOGIN_SUCCESS: {
      localStorage.setItem("jwt", action.payload.data["token"]);
      // localStorage.setItem("expiration", action.payload.data["expiration"]);
      // axios.defaults.headers.common["Authorization"] = `bearer ${
      //   action.payload.data["token"]
      // }`;

      return Object.assign({}, state, {
        data: action.payload.data,
        isLoading: false,
        hasErrored: false,
        errorMessages: [],
      });
    }
    case LOGIN_FAIL: {
      var s = [];
      for (var err in action.error.response.data)
        [...action.error.response.data[err]].forEach((v, i) => s.push(v));

      return Object.assign({}, state, {
        isLoading: false,
        hasErrored: true,
        errorMessages: s,
      });
    }
    case LOGOUT: {
      // localStorage.removeItem("jwt");
      // localStorage.removeItem("expiration");
      axios.defaults.headers.common["Authorization"] = "";
      return state;
    }
    default: {
      return state;
    }
  }
}
