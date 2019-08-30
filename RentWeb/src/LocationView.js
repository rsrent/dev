import React, {Component} from 'react';
import {Link} from "react-router-dom";
import HelperMethods from "./HelperMethods";

let Config = require('./config.json');

class LocationView extends React.Component {
    constructor(props) {
        super(props);
        console.log("perm", localStorage.getItem("permissions"));
        if (props.location.state !== undefined) {
            this.state = {
                locationID: props.match.params.locationID,
                locationInformation: props.location.state.locationInformation,
                documentPermission: JSON.parse(localStorage.getItem('permissions')).find(element => {
                    return element.name.toLocaleLowerCase() === "document"
                })
            };
        } else {
            this.state = {
                locationID: props.match.params.locationID,
                locationInformation: {},
                documentPermission: JSON.parse(localStorage.getItem('permissions')).find(element => {
                    return element.name.toLocaleLowerCase() === "document"
                })
            };
            this.getLocationInformation();
        }
        if (localStorage.getItem("cleaningPlanFolderID") === null) {
            this.getLocationInformation();
        }

    }

    getLocationInformation() {
        let myHeaders = new Headers();
        myHeaders.append("Authorization", "Bearer " + localStorage.getItem('token'));
        HelperMethods.getRequest("Location/" + this.state.locationID, myHeaders,
            (information) => {
                this.setState({locationInformation: information});
            }, undefined, () => {
            });
        // fetch(Config.API_URL + "/Location/" + this.state.locationID, {
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
        //         this.setState({locationInformation: information});
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
        const title = localStorage.getItem('title').toLocaleLowerCase();
        return (
            <div>
                <div className="location-information">
                    {
                        Object.keys(this.state.locationInformation).length ?
                            <div className="location-details-container">
                                <div className="location-details">
                                    <p><span>{this.state.locationInformation.name}</span>
                                        <br/>
                                        {this.state.locationInformation.serviceLeader != null && ["9", "8"].indexOf(localStorage.getItem("roleID")) >= 0 ?
                                            <div>
                                                <svg xmlns="http://www.w3.org/2000/svg"
                                                     xmlnsXlink="http://www.w3.org/1999/xlink"
                                                     version="1.1" width={24} height={24} style={{marginRight: 12}}
                                                     viewBox="0 0 24 24">
                                                    <path
                                                        d="M12,4A4,4 0 0,1 16,8A4,4 0 0,1 12,12A4,4 0 0,1 8,8A4,4 0 0,1 12,4M12,14C16.42,14 20,15.79 20,18V20H4V18C4,15.79 7.58,14 12,14Z"/>
                                                </svg>
                                                {this.state.locationInformation.serviceLeader.firstName} {this.state.locationInformation.serviceLeader.lastName}
                                                <br/>
                                            </div>
                                            :
                                            this.state.locationInformation.customerContact != null ?
                                                <div>
                                                    <svg xmlns="http://www.w3.org/2000/svg"
                                                         xmlnsXlink="http://www.w3.org/1999/xlink"
                                                         version="1.1" width={24} height={24} style={{marginRight: 12}}
                                                         viewBox="0 0 24 24">
                                                        <path
                                                            d="M12,4A4,4 0 0,1 16,8A4,4 0 0,1 12,12A4,4 0 0,1 8,8A4,4 0 0,1 12,4M12,14C16.42,14 20,15.79 20,18V20H4V18C4,15.79 7.58,14 12,14Z"/>
                                                    </svg>
                                                    {this.state.locationInformation.customerContact.firstName} {this.state.locationInformation.customerContact.lastName}
                                                    <br/></div>
                                                :
                                                <div/>
                                        }
                                        <svg xmlns="http://www.w3.org/2000/svg"
                                             xmlnsXlink="http://www.w3.org/1999/xlink"
                                             version="1.1" width={24} height={24} viewBox="0 0 24 24">
                                            <path
                                                d="M12,11.5A2.5,2.5 0 0,1 9.5,9A2.5,2.5 0 0,1 12,6.5A2.5,2.5 0 0,1 14.5,9A2.5,2.5 0 0,1 12,11.5M12,2A7,7 0 0,0 5,9C5,14.25 12,22 12,22C12,22 19,14.25 19,9A7,7 0 0,0 12,2Z"/>
                                        </svg>
                                        <a href={"https://www.google.com/maps/search/" + this.state.locationInformation.address}
                                           target="_blank">{this.state.locationInformation.address}</a>
                                        <br/>
                                        {this.state.locationInformation.serviceLeader != null && ["9", "8"].indexOf(localStorage.getItem("roleID")) >= 0 ?
                                            <div>
                                                <svg xmlns="http://www.w3.org/2000/svg"
                                                     xmlnsXlink="http://www.w3.org/1999/xlink"
                                                     version="1.1" width={20} height={20} style={{marginLeft: 3}}
                                                     viewBox="0 0 24 24">
                                                    <path
                                                        d="M20,8L12,13L4,8V6L12,11L20,6M20,4H4C2.89,4 2,4.89 2,6V18A2,2 0 0,0 4,20H20A2,2 0 0,0 22,18V6C22,4.89 21.1,4 20,4Z"/>
                                                </svg>
                                                <a href={"mailto:" + this.state.locationInformation.serviceLeader.email}>{this.state.locationInformation.serviceLeader.email}</a>
                                                <br/>
                                                <svg xmlns="http://www.w3.org/2000/svg"
                                                     xmlnsXlink="http://www.w3.org/1999/xlink"
                                                     version="1.1" width={22} height={22} style={{marginLeft: 1}}
                                                     viewBox="0 0 24 24">
                                                    <path
                                                        d="M6.62,10.79C8.06,13.62 10.38,15.94 13.21,17.38L15.41,15.18C15.69,14.9 16.08,14.82 16.43,14.93C17.55,15.3 18.75,15.5 20,15.5A1,1 0 0,1 21,16.5V20A1,1 0 0,1 20,21A17,17 0 0,1 3,4A1,1 0 0,1 4,3H7.5A1,1 0 0,1 8.5,4C8.5,5.25 8.7,6.45 9.07,7.57C9.18,7.92 9.1,8.31 8.82,8.59L6.62,10.79Z"/>
                                                </svg>
                                                <a href={"tel:" + this.state.locationInformation.serviceLeader.phone}>{this.state.locationInformation.serviceLeader.phone}</a>
                                            </div>
                                            :
                                            this.state.locationInformation.customerContact != null ?
                                                <div>
                                                    <svg xmlns="http://www.w3.org/2000/svg"
                                                         xmlnsXlink="http://www.w3.org/1999/xlink"
                                                         version="1.1" width={20} height={20} style={{marginLeft: 3}}
                                                         viewBox="0 0 24 24">
                                                        <path
                                                            d="M20,8L12,13L4,8V6L12,11L20,6M20,4H4C2.89,4 2,4.89 2,6V18A2,2 0 0,0 4,20H20A2,2 0 0,0 22,18V6C22,4.89 21.1,4 20,4Z"/>
                                                    </svg>
                                                    <a href={"mailto:" + this.state.locationInformation.customerContact.email}>{this.state.locationInformation.customerContact.email}</a>
                                                    <br/>
                                                    <svg xmlns="http://www.w3.org/2000/svg"
                                                         xmlnsXlink="http://www.w3.org/1999/xlink"
                                                         version="1.1" width={22} height={22} style={{marginLeft: 1}}
                                                         viewBox="0 0 24 24">
                                                        <path
                                                            d="M6.62,10.79C8.06,13.62 10.38,15.94 13.21,17.38L15.41,15.18C15.69,14.9 16.08,14.82 16.43,14.93C17.55,15.3 18.75,15.5 20,15.5A1,1 0 0,1 21,16.5V20A1,1 0 0,1 20,21A17,17 0 0,1 3,4A1,1 0 0,1 4,3H7.5A1,1 0 0,1 8.5,4C8.5,5.25 8.7,6.45 9.07,7.57C9.18,7.92 9.1,8.31 8.82,8.59L6.62,10.79Z"/>
                                                    </svg>
                                                    <a href={"tel:" + this.state.locationInformation.customerContact.phone}>{this.state.locationInformation.customerContact.phone}</a>
                                                </div>
                                                : <div/>
                                        }
                                        {this.state.locationInformation.projectNumber !== null ?
                                            <div>
                                                <svg xmlns="http://www.w3.org/2000/svg"
                                                     xmlnsXlink="http://www.w3.org/1999/xlink"
                                                     version="1.1" width={22} height={22} style={{marginLeft: 1}}
                                                     viewBox="0 0 1792 1792">
                                                    <path
                                                        d="M991 1024l64-256h-254l-64 256h254zm768-504l-56 224q-7 24-31 24h-327l-64 256h311q15 0 25 12 10 14 6 28l-56 224q-5 24-31 24h-327l-81 328q-7 24-31 24h-224q-16 0-26-12-9-12-6-28l78-312h-254l-81 328q-7 24-31 24h-225q-15 0-25-12-9-12-6-28l78-312h-311q-15 0-25-12-9-12-6-28l56-224q7-24 31-24h327l64-256h-311q-15 0-25-12-10-14-6-28l56-224q5-24 31-24h327l81-328q7-24 32-24h224q15 0 25 12 9 12 6 28l-78 312h254l81-328q7-24 32-24h224q15 0 25 12 9 12 6 28l-78 312h311q15 0 25 12 9 12 6 28z"/>
                                                </svg>
                                                {this.state.locationInformation.projectNumber}
                                            </div> : <div/>}
                                    </p>
                                </div>
                            </div>
                            :
                            <div className="location-details">
                                <p><span>Loading</span>
                                    <br/>
                                    <svg xmlns="http://www.w3.org/2000/svg" xmlnsXlink="http://www.w3.org/1999/xlink"
                                         version="1.1" width={24} height={24} style={{marginRight: 12}}
                                         viewBox="0 0 24 24">
                                        <path
                                            d="M12,4A4,4 0 0,1 16,8A4,4 0 0,1 12,12A4,4 0 0,1 8,8A4,4 0 0,1 12,4M12,14C16.42,14 20,15.79 20,18V20H4V18C4,15.79 7.58,14 12,14Z"/>
                                    </svg>
                                    Loading
                                    <br/>
                                    <svg xmlns="http://www.w3.org/2000/svg" xmlnsXlink="http://www.w3.org/1999/xlink"
                                         version="1.1" width={24} height={24} viewBox="0 0 24 24">
                                        <path
                                            d="M12,11.5A2.5,2.5 0 0,1 9.5,9A2.5,2.5 0 0,1 12,6.5A2.5,2.5 0 0,1 14.5,9A2.5,2.5 0 0,1 12,11.5M12,2A7,7 0 0,0 5,9C5,14.25 12,22 12,22C12,22 19,14.25 19,9A7,7 0 0,0 12,2Z"/>
                                    </svg>
                                    <a href=""
                                       target="_blank">Loading</a>
                                    <br/>
                                    <svg xmlns="http://www.w3.org/2000/svg" xmlnsXlink="http://www.w3.org/1999/xlink"
                                         version="1.1" width={20} height={20} style={{marginLeft: 3}}
                                         viewBox="0 0 24 24">
                                        <path
                                            d="M20,8L12,13L4,8V6L12,11L20,6M20,4H4C2.89,4 2,4.89 2,6V18A2,2 0 0,0 4,20H20A2,2 0 0,0 22,18V6C22,4.89 21.1,4 20,4Z"/>
                                    </svg>
                                    <a href="">Loading</a>
                                    <br/>
                                    <svg xmlns="http://www.w3.org/2000/svg" xmlnsXlink="http://www.w3.org/1999/xlink"
                                         version="1.1" width={22} height={22} style={{marginLeft: 1}}
                                         viewBox="0 0 24 24">
                                        <path
                                            d="M6.62,10.79C8.06,13.62 10.38,15.94 13.21,17.38L15.41,15.18C15.69,14.9 16.08,14.82 16.43,14.93C17.55,15.3 18.75,15.5 20,15.5A1,1 0 0,1 21,16.5V20A1,1 0 0,1 20,21A17,17 0 0,1 3,4A1,1 0 0,1 4,3H7.5A1,1 0 0,1 8.5,4C8.5,5.25 8.7,6.45 9.07,7.57C9.18,7.92 9.1,8.31 8.82,8.59L6.62,10.79Z"/>
                                    </svg>
                                    <a href="">Loading</a></p>
                            </div>
                    }
                </div>
                <div className="buttons-group">
                    <div className="button-content">
                        <Link className="btn btn-simp btn-primary select-button" href="#" role="button"
                              to={{
                                  pathname: `${this.props.location.pathname}CleaningPlans/`
                              }}>Rengøringsplaner</Link>
                    </div>
                    <div className="button-content">
                        <Link className="btn btn-simp btn-primary select-button" href="#" role="button"
                              to={{
                                  pathname: `${this.props.location.pathname}QualityReports/`
                              }}>Kvalitetsrapporter</Link>
                    </div>
                    <div className="button-content">
                        <Link className="btn btn-simp btn-primary select-button" href="#" role="button"
                              to={{
                                  pathname: `${this.props.location.pathname}Users/`
                              }}>Holdet</Link>
                    </div>
                    <div className="button-content">
                        <Link className="btn btn-simp btn-primary select-button" href="#" role="button"
                              to={{
                                  pathname: `${this.props.location.pathname}Documents/`
                              }}>Dokumenter</Link>
                    </div>
                    <div className="button-content">
                        <Link className="btn btn-simp btn-primary select-button" href="#" role="button"
                              to={{
                                  pathname: `${this.props.location.pathname}Message/`
                              }}>Skriv til os</Link>
                    </div>
                    <div className="button-content">
                        <Link className="btn btn-simp btn-primary select-button" href="#" role="button"
                              to={{
                                  pathname: `${this.props.location.pathname}Logs/`
                              }}>Logs</Link>
                    </div>
                </div>
            </div>



        );
    }
}

export default LocationView;
