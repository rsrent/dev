import React, {Component} from 'react';
import CleaningTaskElement from "./CleaningTaskElement";
import WindowTaskElement from "./WindowTaskElement";
import HelperMethods from "./HelperMethods";
import {Link} from "react-router-dom";

let Config = require('./config.json');


class DocumentLinkButton extends Component {
    constructor(props) {
        super(props);
        this.state = {
            folderID: -1,
        };
    }

    componentDidMount() {
        let myHeaders = new Headers();
        myHeaders.append("Authorization", "Bearer " + localStorage.getItem('token'));
        HelperMethods.getRequest("Location/" + this.props.locationID, myHeaders,
            (information) => {
                console.log("plantype: " + this.props.planType);
                let planType = parseInt(this.props.planType);
                console.log("plantype int: " + planType);
                if (planType === 1) {
                    this.setState({folderID: information.cleaningFolderID});
                } else if (planType === 2) {
                    this.setState({folderID: information.windowFolderID});
                } else if (planType === 3) {
                    this.setState({folderID: information.fanCoilFolderID});
                } else if (planType === 4) {
                    this.setState({folderID: information.periodicFolderID});
                }
            }, undefined, () => {
                if (typeof this.props.handler !== "undefined") {
                    this.props.handler();
                }
            });
    }

    render() {
        return (
            this.state.folderID !== -1 && this.state.folderID !== null
                ?
                <div className="button-content">
                    <Link className="btn btn-simp btn-secondary select-button" href="#" role="button"
                          to={{
                              pathname: `/Customers/${this.props.customerID}/Locations/${this.props.locationID}/Documents/${this.state.folderID}/`
                          }}>Dokumenter</Link>
                </div>
                :
                null
        );
    }
}

export default DocumentLinkButton;