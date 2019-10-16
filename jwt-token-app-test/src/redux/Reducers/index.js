import { combineReducers } from "redux";
import { Login } from "./Authorization";
import { loadHello } from "./Hello";

export default combineReducers({ Login: Login, Hello: loadHello });
