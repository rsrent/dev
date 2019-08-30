import React, {Component} from 'react';
import {Redirect} from "react-router-dom";

let Config = require('./config.json');

class Footer extends Component {
    constructor(props) {
        super(props);
    }

    render() {
        return (
            <footer className="footer">
                <div className="container-fluid">
                    <p className="small">Rengøringsselskabet RENT ApS | <a href="mailto:info@rs-rent.dk">info@rs-rent.dk</a> | <a href="tel:+4570215500">+45 70 21 55 00</a></p>
                </div>
            </footer>

        );
    }
}

export default Footer;