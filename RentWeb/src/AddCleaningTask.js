import React, {Component} from 'react';
import {Redirect} from "react-router-dom";
import HelperMethods from "./HelperMethods";

let Config = require('./config.json');

class AddCleaningTask extends Component {
    constructor(props) {
        super(props);
        this.state = {plantype:0, floor: 'Kælder', area:'Stuen', SquareMeters: 0, Frequency: 0, TimesOfYear: 0, Comment:''};
        this.createCleaningTask = this.createCleaningTask.bind(this);
        this.handleInputChange = this.handleInputChange.bind(this);
    }


    createCleaningTask(event) {
        event.preventDefault();
        console.log(this.state.floor);
        let cleaningTask = {
            PlanType: this.state.plantype,
            SquareMeters: this.state.SquareMeters,
            Frequency: this.state.Frequency !== 0 ? this.state.Frequency : null,
            TimesOfYear: this.state.TimesOfYear !== 0 ? this.state.TimesOfYear : null,
            Comment: this.state.Comment,
            Floor: { Description: this.state.floor},
            Area: { Description: this.state.area},
        };
        cleaningTask = JSON.stringify(cleaningTask);
        console.log(cleaningTask);
        let myHeaders = new Headers();
        myHeaders.append("Authorization", "Bearer " + localStorage.getItem('token'));
        myHeaders.append("Content-Type", "application/json");
        HelperMethods.postRequest("CleaningTask/Add/" + this.props.match.params.locationID, myHeaders, () => {alert("Cleaningtask oprettet!");}, undefined, cleaningTask);
    }

    handleInputChange(event) {
        const name = event.target.name;
        if(name === "plantype" && event.target.value === "1"){
            this.setState({floor: "vindue"});
            let floor = document.getElementById("floor");
            floor.innerHTML = "<option selected value='vindue'>Vindue</option>";
            floor.setAttribute("disabled", "disabled");
            let area = document.getElementById("area");
            console.log("yoo");
        }
        this.setState({[name]: event.target.value});
    }

    render() {
        return (
            <div>
                <form onSubmit={this.createCleaningTask}>
                    <label>Plantype
                    <select name="plantype" value={this.state.plantype} onChange={this.handleInputChange}>
                        <option value="0">Regular</option>
                    </select>
                    </label><br/>
                    <label>Etage
                        <select id="floor" name="floor" value={this.state.floor} onChange={this.handleInputChange}>
                            <option selected value="Kælder"  >Kælder</option>
                            <option value="Stuen"   >Stuen </option>
                            <option value="1."      >1.    </option>
                            <option value="2."      >2.    </option>
                            <option value="3."      >3.    </option>
                            <option value="4."      >4.    </option>
                            <option value="5."      >5.    </option>
                            <option value="6."      >6.    </option>
                            <option value="7."      >7.    </option>
                            <option value="8."      >8.    </option>
                            <option value="9."      >9.    </option>
                            <option value="10."     >10.   </option>
                            <option value="Custom"  >Custom</option>
                        </select>
                    </label><br/>
                    <label>Område
                        <select id="area" name="area" value={this.state.area} onChange={this.handleInputChange}>
                            <option selected value="Toilet">Toilet</option>
                            <option value="Kontor">Kontor</option>
                            <option value="Mødelokale">Mødelokale</option>
                            <option value="Gang">Gang</option>
                            <option value="Reception">Reception</option>
                            <option value="Trapper">Trapper</option>
                            <option value="Custom">Custom</option>
                        </select>
                    </label><br/>
                    <label>Areal
                        <input type="number" name="SquareMeters" placeholder="Areal" value={this.state.SquareMeters} onChange={this.handleInputChange}/>
                    </label><br/>
                    <label>Frekvens<br/>
                        <input type="number" name="Frequency" placeholder="Frekvens"  value={this.state.Frequency} onChange={this.handleInputChange}/><br/>
                        eller<br/>
                        <select name="TimesOfYear" value={this.state.TimesOfYear} onChange={this.handleInputChange}>
                            <option value="0"> </option>
                            <option value="1">1 gang om året</option>
                            <option value="2">2 gange om året</option>
                            <option value="3">3 gange om året</option>
                            <option value="4">4 gange om året</option>
                            <option value="6">6 gange om året</option>
                            <option value="12">1 gang om måneden</option>
                            <option value="26">1 gang hver anden uge</option>
                            <option value="52">1 gang om ugen</option>
                        </select>
                    </label><br/>
                    <label>Kommentar
                        <textarea name="Comment" value={this.state.Comment} onChange={this.handleInputChange}></textarea>
                    </label><br/>
                    <button type="submit" className="btn btn-simp login-button pointer">Opret</button>
                </form>
            </div>
        );
    }
}
export default AddCleaningTask;
