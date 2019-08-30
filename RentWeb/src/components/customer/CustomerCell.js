import React from 'react';
import {Link} from "react-router-dom";
import ic_email from '../images/ic_email.svg' // relative path to image
import ic_phone from '../images/ic_phone.svg'
import ic_title from '../images/ic_title.svg'
import ic_role from '../images/ic_role.svg'
import ic_edit from '../images/ic_edit.svg'
import ic_customer from '../images/ic_customer.svg'
import ic_employee_number from '../images/ic_employee_number.svg'
import ic_profile from '../images/ic_customer_profile.svg'
import '../cell-style.css';

export const CustomerCell = (props) => {
  return <div key={props.customer.id} className="cell-div flex-container">
    <div className="flex-container">
      <img className="cell-image" src={ic_profile}>

      </img>
    </div>

    <div className="row cell-details">
      <div className="col-12 flex-container">
        <h3>{props.customer.name}</h3>
        {
          props.editable ? <Link to={{ pathname: `/Admin/Customers/${props.customer.id}`, params: { customer: props.customer } }}>
            <img className="edit-icon" src={ic_edit} />
          </Link> : <div></div>
        }

      </div>

      <div className="col-12 col-md-6 cell-detail flex-container">
        <h5 className="detail-info">KAM:</h5>
        <h5>{props.customer.keyAccountManager !== null ? props.customer.keyAccountManager.firstName : ""}</h5>
      </div>
      <div className="col-12 col-md-6 cell-detail flex-container">
        <h5 className="detail-info">CM:</h5>
        <h5>{props.customer.mainUser !== null ? props.customer.mainUser.firstName : ""}</h5>
      </div>
    </div>
  </div>
}
