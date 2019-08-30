import React, {Component} from 'react';
import {BounceLoader} from "react-spinners";

class LoadingIndicator extends Component {
    constructor(props) {
        super(props);
    }

    render() {
        return (
            <div className="center-loader">
                <BounceLoader
                    color={'#005AAD'}
                />
            </div>
        );
    }
}

export default LoadingIndicator;