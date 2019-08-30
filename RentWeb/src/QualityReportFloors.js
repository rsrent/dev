import React, {Component} from 'react';
import {Link} from "react-router-dom";
import HelperMethods from "./HelperMethods";
import {BounceLoader} from "react-spinners";
import LoadingIndicator from "./LoadingIndicator";

let Config = require('./config.json');


class QualityReportFloors extends Component {
    constructor(props) {
        super(props);
        this.state = {
            floors: [],
            comment: '',
            qualityReportID: props.match.params.reportID,
            loaded: false
        };
    }

    componentDidMount() {
        let myHeaders = new Headers();
        myHeaders.append("Authorization", "Bearer " + localStorage.getItem('token'));
        HelperMethods.getRequest("QualityReports/GetPlanWithFloors/" + this.state.qualityReportID, myHeaders,
            (floors) => {
                this.setState({floors: floors.floors, comment: floors.comment});
            }, undefined, () => {
                this.setState({loaded: true});
            });
    }

    render() {
        return (
            <div className="buttons-group">
              <div className="button-content">
                  <Link className="btn btn-simp btn-secondary select-button" href="#" role="button"
                        to={{
                            pathname: `${this.props.location.pathname}CustomerComment/`,
                            state: {comment: this.state.comment}
                        }}>Kommentar</Link>
              </div>
                {
                    this.state.loaded && this.state.floors.length > 0 ?
                        this.state.floors.map(floor =>
                            <div className="button-content" key={floor.floor.id}>
                                {console.log("floor", floor.areas)}
                                <Link className="btn btn-simp btn-primary select-button" href="#" role="button"
                                      to={{
                                          pathname: `${this.props.location.pathname}${floor.floor.id}/`,
                                          state: {areas: floor.areas}
                                      }}>{floor.floor.description}</Link>
                            </div>
                        )
                        : this.state.loaded ? <p>Der er ikke nogle etager til denne lokation.</p> :
                        <LoadingIndicator/>}
            </div>
        );
    }
}

export default QualityReportFloors;
//http://jsfiddle.net/weqm8q5w/6/
