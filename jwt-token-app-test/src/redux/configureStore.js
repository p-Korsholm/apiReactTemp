import {createStore, applyMiddleware} from 'redux';
import thunk from 'redux-thunk';
import Axios from 'axios';
import reducers from './Reducers';
import axiosMiddleware from 'redux-axios-middleware';
import {composeWithDevTools} from 'redux-devtools-extension';
 
const client = Axios.create({
    baseURL:'http://localhost:3000/',
    responseType: 'json'
  });

export default function configureStore(initialState={}) {
    const composeEnhancers = composeWithDevTools({});

    return createStore(
            reducers, 
            initialState,
            composeEnhancers(applyMiddleware(thunk, axiosMiddleware(client)))
        );
    }