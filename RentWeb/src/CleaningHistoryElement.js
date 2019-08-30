import React, {Component} from 'react';

let Config = require('./config.json');

class CleaningHistoryElement extends Component {
    constructor(props) {
        super(props);
    }

    render() {
        let pStyle = {float: 'left', marginBottom: 0};
        let imgStyle = {marginRight: '5px', borderRadius: '50%'};
        return (
            <div className="reg-cp-card-content" key={this.props.task.id}>
                <div className="reg-cp-card">
                    <div className="reg-cp-card-header history">
                        <div className="reg-cp-card-title">

                            <h2>{this.props.task.completedByUser.firstName} {this.props.task.completedByUser.lastName}</h2>
                        </div>
                    </div>
                    <div className="reg-cp-card-details d-flex justify-content-between">
                        <div className="reg-cp-card-details-code">
                            <p style={pStyle}>
                                {this.props.task.comment}
                            </p>
                        </div>
                        <div className="reg-cp-card-details-size">
                            <p>{new Date(this.props.task.completedDate).toLocaleDateString()}</p>
                        </div>
                    </div>
                </div>
            </div>


        );
    }
}

export default CleaningHistoryElement;