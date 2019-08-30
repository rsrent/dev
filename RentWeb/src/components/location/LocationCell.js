import React from 'react';
import {Link} from "react-router-dom";
import ic_email from '../images/ic_email.svg'
import ic_phone from '../images/ic_phone.svg'
import ic_title from '../images/ic_title.svg'
import ic_role from '../images/ic_role.svg'
import ic_edit from '../images/ic_edit.svg'
import ic_customer from '../images/ic_customer.svg'
import ic_address from '../images/ic_address.svg'
import ic_profile from '../images/ic_location_profile.svg'
import ic_employee_number from '../images/ic_employee_number.svg'
import '../cell-style.css';

export const LocationCell = (props) => {
  return <div className="cell-div flex-container">
    <div className="flex-container">
      <img className="cell-image" src={ic_profile}>

      </img>
    </div>

    <div className="row cell-details">
      <div className="col-12 flex-container">
        <h3>{props.location.name}</h3>
          {
            props.editable ? <Link to={{ pathname: `/Admin/Locations/${props.location.id}`, params: { location: props.location } }}>
              <img className="edit-icon" src={ic_edit} />
            </Link> : <div></div>
          }
      </div>
      <div className="col-12 col-md-6 cell-detail flex-container">
        <img src={ic_customer}></img>
        <h5>{props.location.customer.name}</h5>
      </div>
      <div className="col-12 col-md-6 cell-detail flex-container">
        <img src={ic_address}></img>
        <h5>{props.location.address}</h5>
      </div>
      <div className="col-12 col-md-6 cell-detail flex-container">
        <h5 className="detail-info">SL:</h5>
        <h5>{props.location.serviceLeader !== null ? props.location.serviceLeader.firstName : ""}</h5>
      </div>
      <div className="col-12 col-md-6 cell-detail flex-container">
        <h5 className="detail-info">LM:</h5>
        <h5>{props.location.customerContact !== null ? props.location.customerContact.firstName : ""}</h5>
      </div>
    </div>
  </div>
}
