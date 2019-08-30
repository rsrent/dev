import React from 'react';
import {Link} from "react-router-dom";
import ic_email from '../images/ic_email.svg' // relative path to image
import ic_phone from '../images/ic_phone.svg'
import ic_title from '../images/ic_title.svg'
import ic_role from '../images/ic_role.svg'
import ic_edit from '../images/ic_edit.svg'
import ic_customer from '../images/ic_customer.svg'
import ic_employee_number from '../images/ic_employee_number.svg'
import ic_profile from '../images/ic_user_profile.svg'
import '../cell-style.css';

export const UserCell = (props) => {
  return <div key={props.user.id} className="cell-div flex-container">
    <div className="flex-container">
      <img className="cell-image" src={ic_profile}>

      </img>
    </div>

    <div className="row cell-details">
      <div className="col-12 flex-container">
        <h3>{props.user.firstName + ' ' + props.user.lastName}</h3>

          {
            props.editable ? <Link to={{ pathname: `/Admin/Users/${props.user.id}`, params: { user: props.user } }}>
              <img className="edit-icon" src={ic_edit} />
            </Link> : <div></div>
          }
      </div>
      {
        props.user.customer != null ? (<div className="col-12 col-md-6 cell-detail flex-container">
                                <img src={ic_customer}></img>
                                <h5>{props.user.customer.name}</h5>
                              </div>) : (
                              <div className="col-12 col-md-6 cell-detail flex-container">
                                <img src={ic_employee_number}></img>
                                <h5>{props.user.employeeNumber}</h5>
                              </div>)
      }


      <div className="col-12 col-md-6 cell-detail flex-container">
        <img src={ic_title}></img>
        <h5>{props.user.title}</h5>
      </div>
      <div className="col-12 col-md-6 cell-detail flex-container">
        <img className="svg-icon" src={ic_role}></img>
        <h5>{props.user.role.name}</h5>
      </div>
      <div className="col-12 col-md-6 cell-detail flex-container">
        <img src={ic_phone}></img>
        <h5>{props.user.phone}</h5>
      </div>
      <div className="col-12 col-md-6 cell-detail flex-container">
        <img src={ic_email}></img>
        <h5>{props.user.email}</h5>
      </div>
    </div>
  </div>
}
