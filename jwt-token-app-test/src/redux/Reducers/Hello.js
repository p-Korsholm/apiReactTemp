import {
  HELLO_LOAD,
  HELLO_LOAD_SUCCESS,
  HELLO_LOAD_FAIL,
} from "../Actions/Hello";

import axios from "axios";

export function loadHello(
  state = { data: [], isLoading: false, hasErrored: false, errors: [] },
  action
) {
  switch (action.type) {
    case HELLO_LOAD: {
      console.log(action);
      return Object.assign(
        {},
        {
          isLoading: true,
          hasErrored: false,
          errorMessages: [],
        }
      );
    }
    case HELLO_LOAD_SUCCESS: {
      return Object.assign({}, state, {
        data: action.payload.data,
        isLoading: false,
        hasErrored: false,
        errorMessages: [],
      });
    }
    case HELLO_LOAD_FAIL: {
      const s = [];
      s.push([
        `${action.error.response.status} ${action.error.response.statusText}`,
      ]);
      return Object.assign({}, state, {
        isLoading: false,
        hasErrored: true,
        errorMessages: s,
      });
    }
    default: {
      return state;
    }
  }
}
