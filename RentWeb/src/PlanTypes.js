import React, {Component} from 'react';
import {Link} from "react-router-dom";
import ShowFiles from "./ShowFiles";
import ShowBlobs from "./ShowBlobs";
import HelperMethods from "./HelperMethods";
import LoadingIndicator from "./LoadingIndicator";

let Config = require('./config.json');

class PlanTypes extends Component {
    constructor(props) {
        super(props);
        this.state = {
            locationID: props.match.params.locationID,
            cleaningPlan: {},
            cleaningPlanType: "",
            floors: [],
            cleaningTasks: [],
            loaded: false,
            cleaningPlanFolderID: -1
        };
    }

    async getLocationInformation() {
        let myHeaders = new Headers();
        myHeaders.append("Authorization", "Bearer " + localStorage.getItem('token'));
        HelperMethods.getRequest("Location/" + this.state.locationID, myHeaders,
            (information) => {
                this.setState({cleaningPlanFolderID: information.cleaningplanFolderID + "-folder"});
            }, undefined, () => {
            });
    }

    async componentDidMount() {
        let myHeaders = new Headers();
        myHeaders.append("Authorization", "Bearer " + localStorage.getItem('token'));
        await this.getLocationInformation();
        HelperMethods.getRequest("CleaningTask/Plans/" + this.state.locationID, myHeaders,
            (cleaningPlan) => {
                this.setState({cleaningPlan: cleaningPlan, loaded: true});
            });
    }

    render() {
        const cleaningPlan = this.state.cleaningPlan;
        return (
            <div className="buttons-group">
                {
                    this.state.loaded && cleaningPlan.length ?
                        <div>
                            {cleaningPlan.map(plan =>
                                <div className="button-content" key={plan.planType}>
                                    {console.log(plan)}
                                    {plan.cleaningPlan.hasFloors
                                        ?
                                        <Link className="btn btn-simp btn-primary select-button" href="#" role="button"
                                              to={{
                                                  pathname: `${this.props.location.pathname}${plan.cleaningPlan.id}/Floors/`,
                                                  state: {floors: plan.floors}
                                              }}>{plan.cleaningPlan.description}</Link>
                                        :
                                        <Link className="btn btn-simp btn-primary select-button" href="#" role="button"
                                              to={{
                                                  pathname: `${this.props.location.pathname}${plan.cleaningPlan.id}/${plan.cleaningPlan.description}/`,
                                                  state: {tasks: plan.areas}
                                              }}>{plan.cleaningPlan.description}</Link>
                                    }
                                </div>
                            )}
                            {/*<ShowBlobs containerName={this.state.cleaningPlanFolderID}/>*/}
                        </div>
                        :
                        this.state.loaded
                            ? <p>Der er ikke nogle rengøringsplaner til denne lokation</p>
                            : <LoadingIndicator/>
                }
            </div>
        );
    }

}

export default PlanTypes;
