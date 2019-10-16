import { createStore, applyMiddleware } from "redux";
import thunk from "redux-thunk";
import Axios from "axios";
import reducers from "./Reducers";
import axiosMiddleware from "redux-axios-middleware";
import { composeWithDevTools } from "redux-devtools-extension";

const client = Axios.create({
  baseURL: "http://localhost:3000/",
  responseType: "json",
});

const middlewareConfig = {
  interceptors: {
    request: [
      {
        success: function({ getState, dispatch, getSourceAction }, req) {
          console.log(req); //contains information about request object
          if (localStorage.getItem("jwt"))
            req.headers["authorization"] = `Bearer ${localStorage.getItem(
              "jwt"
            )}`;
          return req;
        },
      },
    ],
    response: [
      {
        success: function({ getState, dispatch, getSourceAction }, res) {
          return Promise.resolve(res);
        },
        error: function({ getState, dispatch, getSourceAction }, error) {
          return Promise.reject(error);
        },
      },
    ],
  },
};

export default function configureStore(initialState = {}) {
  const composeEnhancers = composeWithDevTools({});

  return createStore(
    reducers,
    initialState,
    composeEnhancers(
      applyMiddleware(thunk, axiosMiddleware(client, middlewareConfig))
    )
  );
}
