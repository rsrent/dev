import React, {Component} from 'react';
import {Link} from "react-router-dom";
import HelperMethods from "./HelperMethods";
import LoadingIndicator from "./LoadingIndicator";

let Config = require('./config.json');

class ListQualityReports extends Component {
    constructor(props) {
        super(props);
        this.state = {
            locationID: props.match.params.locationID,
            qualityReports: [],
            loaded: false
        };
        console.log(this.props);
    }

    componentDidMount() {
        let myHeaders = new Headers();
        myHeaders.append("Authorization", "Bearer " + localStorage.getItem('token'));
        HelperMethods.getRequest("QualityReports/GetForLocation/" + this.state.locationID, myHeaders,
            (reports) => {
                this.setState({qualityReports: reports, loaded: true});
            });
        // fetch(Config.API_URL + "/QualityReports/GetForLocation/" + this.state.locationID, {
        //     method: "GET",
        //     headers: myHeaders
        // })
        //     .then(result => {
        //         if (!result.ok) {
        //             throw result;
        //         }
        //         return result.json();
        //     })
        //     .then(reports => {
        //         this.setState({qualityReports: reports, loaded: true});
        //     })
        //     .catch(error => {
        //         if (error.status === 404) {
        //             throw error;
        //         } else if (error.status === 401) {
        //             localStorage.clear();
        //             window.location.replace("/");
        //         } else {
        //             alert("Der skete en ukendt serverfejl");
        //         }
        //     })
        //     .catch(async errorResult => {
        //         let result = await errorResult.json();
        //         alert(result);
        //     });
    }

    render() {
        return (
            <div className="buttons-group">
                {
                    this.state.loaded && this.state.qualityReports.length ?
                        this.state.qualityReports.map(report =>
                            <div className="button-content" key={report.id}>
                                <Link className="btn btn-simp btn-primary select-button" href="#" role="button"
                                      to={{
                                          pathname: `${this.props.location.pathname}${report.id}/Floors/`
                                      }}>{new Date(report.time).toLocaleDateString()}</Link>
                            </div>)
                        :
                        this.state.loaded
                            ? <p>Der er ikke nogle kvalitetsrapporter til denne lokation</p>
                            : <LoadingIndicator/>
                }
            </div>
        );
    }

}

export default ListQualityReports;