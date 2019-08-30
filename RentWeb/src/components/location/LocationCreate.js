import React, { Component } from 'react';
import { CustomerCell } from '../customer/CustomerCell'
import { UserCell } from '../user/UserCell'
import { GetCustomers, GetUsers, UpdateLocation, CreateLocation } from '../api/API.js'
import ic_back from '../images/ic_back.svg'

import '../create-style.css';

export class LocationCreate extends Component {
 constructor(props) {
   super(props);

   let location = { projectNumber: '', name: '', address: '', phone: '', serviceLeaderInterval: '', comment: '',
                  customerID: null, customer: {id: 0, name: ''}, serviceLeaderID: null, serviceLeader: { id: 0, firstName: ''}, customerContactID: null, customerContact: { id: 0, firstName: ''}, };
   let newLocation = true;

   if(props.location.params !== undefined)
   {
      location = props.location.params.location;
      newLocation = false;
   }

   this.state = {
     newLocation: newLocation,
     location: location,
     isLoading: false,
     showCustomers: false,
     customers: [],
     showSlUsers: false,
     showLmUsers: false,
     users: []
   };

   this.handleChange = this.handleChange.bind(this);
   this.update = this.update.bind(this);
   this.create = this.create.bind(this);
   this.updateState = this.updateState.bind(this);
   this.customerSelected = this.customerSelected.bind(this);
   this.serviceLeaderSelected = this.serviceLeaderSelected.bind(this);
   this.locationManagerSelected = this.locationManagerSelected.bind(this);
 }

 updateState(obj) { this.setState(obj); }

 customerSelected(customer) {
   if(customer === null)
   {
     this.state.location.customer = {id: 0, name: ''};
     this.state.location.customerID = null;
   } else {
     this.state.location.customer = customer;
     this.state.location.customerID = customer.id;
   }
   this.setState({
     showCustomers: false, location: this.state.location
   });
 }

 serviceLeaderSelected(user) {
   if(user === null)
   {
     this.state.location.serviceLeader = {id: 0, name: ''};
     this.state.location.serviceLeaderID = null;
   } else {
     this.state.location.serviceLeader = user;
     this.state.location.serviceLeaderID = user.id;
   }
   this.setState({
     showSlUsers: false, location: this.state.location
   });
 }

 locationManagerSelected(user) {
   if(user === null)
   {
     this.state.location.customerContact = {id: 0, name: ''};
     this.state.location.customerContactID = null;
   } else {
     this.state.location.customerContact = user;
     this.state.location.customerContactID = user.id;
   }
   this.setState({
     showLmUsers: false, location: this.state.location
   });
 }

 handleChange(event)
 {
   let name = event.target.name;
   let value = event.target.value;

   if(name === 'showCustomer')
   {
     GetCustomers(this.updateState);
     this.state.showCustomers = true;
   }
   else if(name === 'showSlUsers')
   {
     GetUsers(this.updateState);
     this.state.showSlUsers = true;
   }
   else if(name === 'showLmUsers')
   {
     GetUsers(this.updateState);
     this.state.showLmUsers = true;
   }
   else {
     this.state.location[name] = value;
   }
   this.setState({
     location: this.state.location
   });

 }

 create(event) {
   event.preventDefault();
   CreateLocation(this.state.location, this.updateState);
 }

 update(event) {
   event.preventDefault();
   UpdateLocation(this.state.location, this.updateState);
 }

 render() {
 return (
   this.state.showCustomers ?
   <div className="page-container">
     <div className="flex-container">
       <button className="back-button" onClick={() => this.setState({ showCustomers: false})}>
         <img className="back-icon" src={ic_back} />
       </button>
       <h3>Select a Customer</h3>
     </div>
     {
       this.state.customers.map(c =>
        <div key={c.id} onClick={() => this.customerSelected(c) } >
          <CustomerCell customer={c} />
        </div>
        )
      }
   </div>
   : this.state.showSlUsers ?
   <div className="page-container">
     <div className="flex-container">
       <button className="back-button" onClick={() => this.setState({ showSlUsers: false})}>
         <img className="back-icon" src={ic_back} />
       </button>
       <h3>Select a Service Leader</h3>
     </div>
     {
       this.state.users.map(u =>
        <div key={u.id} onClick={() => this.serviceLeaderSelected(u) } >
          <UserCell user={u} />
        </div>
        )
      }
   </div>
   : this.state.showLmUsers ?
   <div className="page-container">
     <div className="flex-container">
       <button className="back-button" onClick={() => this.setState({ showLmUsers: false})}>
         <img className="back-icon" src={ic_back} />
       </button>
       <h3>Select a Location manager</h3>
     </div>
     {
       this.state.users.map(u =>
        <div key={u.id} onClick={() => this.locationManagerSelected(u) } >
          <UserCell user={u} />
        </div>
        )
      }
   </div>
    : <form className="row page-container" onSubmit={ this.state.newLocation ? this.create : this.update}>
      <h2 className="col-12">New location</h2>
      <div className="col-12 row">
        <h4 className="col-12 col-md-4" >Project number</h4>
        <input name="projectNumber" className="col-12 col-md-8" type="text" value={this.state.location.projectNumber} onChange={this.handleChange}/>
      </div>
      <div className="col-12 row">
        <h4 className="col-12 col-md-4" >Name</h4>
        <input name="name" className="col-12 col-md-8" type="text" required value={this.state.location.name} onChange={this.handleChange}/>
      </div>
      <div className="col-12 row">
        <h4 className="col-12 col-md-4" >Address</h4>
        <input name="address" className="col-12 col-md-8" type="text" required value={this.state.location.address} onChange={this.handleChange}/>
      </div>
      <div className="col-12 row">
        <h4 className="col-12 col-md-4" >Phone</h4>
        <input name="phone" className="col-12 col-md-8" type="tel" required value={this.state.location.phone} onChange={this.handleChange}/>
      </div>
      <div className="col-12 row">
        <h4 className="col-12 col-md-4" >Comment</h4>
        <textarea name="comment" className="col-12 col-md-8" type="text" value={this.state.location.comment} onChange={this.handleChange}/>
      </div>

      <div className="col-12 row">
        <h4 className="col-12 col-md-4" >Customer</h4>
        <input name="showCustomer" className="col-12 col-md-8" required type="button" value={this.state.location.customer.name} onClick={this.handleChange}/>
      </div>
      <div className="col-12 row">
        <h4 className="col-12 col-md-4" >Service leader</h4>
        <input name="showSlUsers" className="col-12 col-md-8" required type="button" value={this.state.location.serviceLeader.firstName} onClick={this.handleChange}/>
      </div>
      <div className="col-12 row">
        <h4 className="col-12 col-md-4" >Location manager</h4>
        <input name="showLmUsers" className="col-12 col-md-8" required type="button" value={this.state.location.customerContact.firstName} onClick={this.handleChange}/>
      </div>

      <div className="col-12 row">
        <div className="col-12 col-md-4"></div>
        <div className="submit-div col-12 col-md-8">
          <button className="submit-button" type="submit">Create location</button>
        </div>
      </div>
   </form>
 )
 }
}
