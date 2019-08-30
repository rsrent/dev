import React, {Component} from 'react';
import { Redirect } from 'react-router'

import {Link} from "react-router-dom";
import HelperMethods from "./HelperMethods";

let Config = require('./config.json');

class CustomerComment extends Component {


  constructor(props) {
      super(props);

      this.state = {comment: "", reportID: props.match.params.reportID, navigate: false, comment: props.location.state.comment,};

      this.handleInputChange = this.handleInputChange.bind(this);
      this.sendMessage = this.sendMessage.bind(this);
  }

  sendMessage(event) {
      event.preventDefault();

      let title = "Customer opinion of collaboration";
      let comment = JSON.stringify(this.state.comment);
      console.log(comment);
      let myHeaders = new Headers();
      myHeaders.append("Authorization", "Bearer " + localStorage.getItem('token'));
      myHeaders.append("Content-Type", "application/json");
      HelperMethods.getRequest("QualityReports/UpdateCustomerComment/" + this.props.match.params.reportID + "/" + comment, myHeaders,
          () => {
              alert("Tak for din besked.");
              this.setState({ navigate: true });
          }, undefined);
  }

  handleInputChange(event) {
      const name = event.target.name;
      this.setState({[name]: event.target.value});
  }

  render() {

      let labelStyle = {width: '100px', height: '100px'};

      if (this.state.navigate) {
        return <Redirect to={this.props.location.pathname.substring(0, this.props.location.pathname.length - 16)} push={true} />
      }

      return (
          <div className="quality-report-card-content">
              <div className="quality-report-card" id="quality-report-card1">
                  <div className="quality-report-card-header">
                      <div className="quality-report-card-title">
                          <h2>Skriv en kommentar til kvalitetsrapporten</h2>
                      </div>
                  </div>
                  {/* COMMENT */}
                  <div className="quality-report-card-comment-wrapper">
                    <textarea placeholder="Skriv din kommentar her"
                              style={{marginTop: "5px", width: "100%", fontSize: "11pt", height: '100px'}}
                              name="comment" value={this.state.comment} onChange={this.handleInputChange}/>
                  </div>
                  {/* /COMMENT */}
                  <button type="submit" className="btn btn-simp select-button" style={{borderRadius: 0, marginLeft: "30%", marginRight: "30%", marginBottom: "5px"}} onClick={this.sendMessage}>Send
                  </button>
              </div>
          </div>

      );
  }
}

export default CustomerComment;
