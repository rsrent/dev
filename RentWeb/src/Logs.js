import React, { Component } from 'react';
import { Link } from "react-router-dom";
import HelperMethods from "./HelperMethods";
import MySearchBar from "./MySearchBar";
import LoadingIndicator from "./LoadingIndicator";

let Config = require('./config.json');

class Logs extends Component {
    constructor(props) {
        super(props);
        this.state = {
            locationID: typeof props.match.params.locationID,
            logs: [],
            loaded: false,
            customerName: "",
        };
    }

    componentDidMount() {
        let myHeaders = new Headers();
        myHeaders.append("Authorization", "Bearer " + localStorage.getItem('token'));
        let fetchURL = "Log/Many/" + this.props.match.params.locationID;
        HelperMethods.getRequest(fetchURL, myHeaders, (logs) => {
            this.setState({ logs: logs });
        }, undefined, () => {
            this.setState({ loaded: true });
        });
    }

    render() {
        const logs = this.state.logs;
        return (
            <div>
                <div className="buttons-group top-margin">
                    {
                        logs.length && this.state.loaded ?
                            logs.map(log =>
                                <div className="log-content" key={log.id}>
                                    <h4>{(log.title != null && log.title.length > 0 ? log.title : 'Log')}</h4>
                                    <p>{log.log != null && log.log.length > 0 ? log.log : '...'}</p>
                                    <p className="log-date">{(new Date(log.dateCreated).toLocaleDateString("da-DK"))}</p>
                                </div>)
                            :
                            this.state.loaded
                                ? <p>Der er ikke nogle logs</p>
                                : <LoadingIndicator />
                    }
                </div>
            </div>
        );
    }
}

export default Logs;
