import React, {Component} from 'react';

let Config = require('./config.json');

class HelperMethods extends Component {

    static defaultErrorHandler(error) {
        console.error("Error-: " + error);
        console.error("Status-: " + error.status);
        if (error.status === 404) {
            alert("Not found");
        } else if (error.status === 403) {
            alert("Din session er udløbet. Login igen.");
            localStorage.clear();
            window.location.replace("/");
        } else if (error.status === 401) {
            alert("Du har ikke adgang til denne funktion. Prøv at logge ud og ind igen.");
        } else {
            alert("Der skete en ukendt serverfejl. Hvis fejlen fortsætter så prøv at logge ud og derefter ind igen.");
        }
    }

    static getRequest(url, headers, successHandler, errorHandler, finalHandler) {
        HelperMethods.genericRequestWithoutBody(url, headers, successHandler, errorHandler = HelperMethods.defaultErrorHandler, "GET", finalHandler)
    }

    static deleteRequest(url, headers, successHandler, errorHandler, finalHandler) {
        HelperMethods.genericRequestWithoutBody(url, headers, successHandler, errorHandler = HelperMethods.defaultErrorHandler, "DELETE", finalHandler)
    }

    static postRequest(url, headers, successHandler, errorHandler = HelperMethods.defaultErrorHandler, requestJSON, finalHandler) {
        HelperMethods.genericRequestWithBody(url, headers, successHandler, errorHandler, requestJSON, "POST", finalHandler);
    }

    static putRequest(url, headers, successHandler, errorHandler = HelperMethods.defaultErrorHandler, requestJSON, finalHandler) {
        HelperMethods.genericRequestWithBody(url, headers, successHandler, errorHandler, requestJSON, "PUT", finalHandler);
    }

    static genericRequestWithoutBody(url, headers, successHandler, errorHandler = HelperMethods.defaultErrorHandler, method, finalHandler = () => {
    }) {
        fetch(Config.API_URL + "/" + url, {
            method: method,
            headers: headers,
        })
            .then(result => {
                if (!result.ok) {
                    throw result;
                }
                console.log(result);
                if (result.status === 204) {
                    return true;
                }
                return result.json()
            })
            .then(result => {
                successHandler(result);
            })
            .catch(error => {
                errorHandler(error);
            })
            .finally(() => {
                finalHandler();
            });

    }

    static genericRequestWithBody(url, headers, successHandler, errorHandler = HelperMethods.defaultErrorHandler, requestJSON, method, finalHandler = () => {
    }) {
        fetch(Config.API_URL + "/" + url, {
            method: method,
            headers: headers,
            body: requestJSON
        })
            .then(result => {
                if (!result.ok) {
                    throw result;
                }
                if (result.statusCode === 204) {
                    return true;
                }
                return result.json()
            })
            .then(result => {
                successHandler(result);
            })
            .catch(error => {
                errorHandler(error);
            })
            .finally(() => {
                finalHandler();
            });
    }
}

export default HelperMethods;
