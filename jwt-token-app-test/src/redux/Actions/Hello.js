export const HELLO_LOAD = "HELLO_LOAD";
export const HELLO_LOAD_SUCCESS = "HELLO_LOAD_SUCCESS";
export const HELLO_LOAD_FAIL = "HELLO_LOAD_FAIL";

export function getHello() {
  return {
    type: HELLO_LOAD,
    payload: {
      request: {
        url: "http://localhost:5000/api/values",
        method: "GET",
      },
    },
  };
}
