import React, {Component} from 'react';
import {Link} from "react-router-dom";
import CleaningHistoryElement from "./CleaningHistoryElement";
import HelperMethods from "./HelperMethods";
import LoadingIndicator from "./LoadingIndicator";

let Config = require('./config.json');

class CleaningHistory extends React.Component {
    constructor(props) {
        super(props);
        this.state = {completedReports: [], loaded: false}

    }

    componentDidMount() {
        let myHeaders = new Headers();
        myHeaders.append("Authorization", "Bearer " + localStorage.getItem('token'));
        HelperMethods.getRequest("CompletedTask/" + this.props.match.params.taskID, myHeaders,
            (information) => {
                this.setState({completedReports: information, loaded: true});
            }, undefined, () => {
            });
        // fetch(Config.API_URL + "/CompletedTask/" + this.props.match.params.taskID, {
        //     method: "GET",
        //     headers: myHeaders
        // })
        //     .then(function (result) {
        //         if (!result.ok) {
        //             throw result;
        //         }
        //         return result.json();
        //     })
        //     .then(information => {
        //         this.setState({completedReports: information, loaded: true});
        //     })
        //     .catch(error => {
        //         console.error(error);
        //         if (error.status === 404) {
        //             alert("Den givne lokation findes ikke");
        //         } else if (error.status === 401) {
        //             localStorage.clear();
        //             window.location.replace("/");
        //         } else {
        //             alert("Der skete en ukendt serverfejl");
        //         }
        //     });
    }


    render() {
        return (
            <div className="top-margin">
                {this.state.loaded
                    ? this.state.completedReports.map(completedReport =>
                        <CleaningHistoryElement task={completedReport}/>
                    )
                    : <LoadingIndicator/>}
                {this.state.loaded && this.state.completedReports.length === 0 &&
                <p>Denne opgave er ikke blevet udført endnu</p>
                }
            </div>
        );
    }
}

export default CleaningHistory;
