export const LOGIN = "LOGIN";
export const LOGIN_SUCCESS = "LOGIN";
export const LOGIN_FAIL = "LOGIN";

export const LOGOUT = "LOGOUT";
export const LOGOUT_SUCCESS = "LOGOUT";
export const LOGOUT_FAIL = "LOGOUT";

export function login(loginModel){
    return {
        type:LOGIN,
        payload:{
            request:{
                
            }
        }
    }
}
