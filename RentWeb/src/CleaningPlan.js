import React, {Component} from 'react';
import {Link, Redirect} from "react-router-dom";
import HelperMethods from "./HelperMethods";
import DocumentLinkButton from "./DocumentLinkButton";
import LoadingIndicator from "./LoadingIndicator";

let Config = require('./config.json');


class CleaningPlan extends Component {
    constructor(props) {
        super(props);
        if (props.location.state !== undefined) {
            this.state = {
                floors: props.location.state.floors,
                locationID: props.match.params.locationID,
                planType: props.match.params.type,
                floorID: props.match.params.floor,
                redirect: false,
                loaded: true,
                documentButtonLoaded: false,
            }
        } else {
            this.state = {
                floors: [],
                locationID: props.match.params.locationID,
                planType: props.match.params.type,
                floorID: props.match.params.floor,
                redirect: false,
                loaded: false,
                documentButtonLoaded: false,
            };
            this.getFloors();
        }
    }

    getFloors() {
        let myHeaders = new Headers();
        myHeaders.append("Authorization", "Bearer " + localStorage.getItem('token'));
        HelperMethods.getRequest("CleaningTask/Floors/" + this.state.locationID + "/" + this.state.planType, myHeaders,
            (floors) => {
                this.setState({floors: floors, loaded: true});
            }, undefined, () => {
            });
    }

    render() {
        //Sample url: /Customers/1/Locations/6/CleaningPlans/2/Vinduespudsning/
        let urlArray = this.props.match.url.split("/");
        let customerIDPostion = urlArray.findIndex((s) => s === "Customers") + 1; //Find the index of customers in the URL string and add 1 to get the index of the the customer position.
        let customerID = urlArray[customerIDPostion];
        return (
            <div className="buttons-group top-margin">
                {this.state.redirect &&
                <Redirect
                    to={"/Locations/User/" + this.props.match.params.userID + "/" + this.props.match.params.locationID + "/CleaningPlan"}/>}

                <DocumentLinkButton locationID={this.state.locationID} planType={this.state.planType}
                                    customerID={customerID} handler={() => this.setState({documentButtonLoaded: true})}/>
                {/*console.log(this.props.location.state.floors)*/}
                {
                    this.state.loaded  && this.state.documentButtonLoaded && this.state.floors.length > 0 ?
                        this.state.floors.map(floor =>
                            <div className="button-content" key={floor.floor.id}>
                                <Link className="btn btn-simp btn-primary select-button" href="#" role="button"
                                      to={{
                                          pathname: `${this.props.location.pathname}${floor.floor.id}/`,
                                          state: {tasks: floor.areas}
                                      }}>{floor.floor.description}</Link>
                            </div>
                        ) : <LoadingIndicator/>
                }
                {this.state.loaded && this.state.floors.length === 0 &&
                <p>Der er ikke nogle etager til denne rengøringsplan</p>
                }
            </div>
        );
    }
}

export default CleaningPlan;
//http://jsfiddle.net/weqm8q5w/6/
