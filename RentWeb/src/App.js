import React, {Component} from 'react';
import './App.css';
import Navbar from "./Navbar";
import PlanTypes from "./PlanTypes";
import MessageOffice from "./MessageOffice";
import {Route, Switch} from "react-router-dom";
import CleaningPlan from "./CleaningPlan";
import CleaningTask from "./CleaningTask";
import Login from "./Login";
import Locations from "./Locations";
import Logs from "./Logs";
import QualityReport from "./QualityReport";
import ListQualityReports from "./ListQualityReports";
import LocationView from "./LocationView";
import QualityReportFloors from "./QualityReportFloors";
import Team from "./Team";
import Footer from "./Footer";
import AddCleaningTask from "./AddCleaningTask";
import CleaningHistory from "./CleaningHistory";
import ShowFiles from "./ShowFiles";
import RateCollaboration from "./RateCollaboration";
import UserProfile from "./UserProfile";
import ShowCustomers from "./ShowCustomers";
import ChangePassword from "./ChangePassword";

import { AdminNavBar } from './components/navigation/AdminNavBar'
import { UserCreate } from './components/user/UserCreate'
import { UserList } from './components/user/UserList'
import { LocationCreate } from './components/location/LocationCreate'
import { LocationList } from './components/location/LocationList'
import { CustomerCreate } from './components/customer/CustomerCreate'
import { CustomerList } from './components/customer/CustomerList'

import CustomerComment from "./CustomerComment";

class App extends Component {
    render() {
        return (
            <div>
                <Navbar/>
                {/*<AdminNavBar/>*/}
                <Route exact path='/' component={Login}/>
                <main role="main" className="container-fluid">
                    <div className="row">
                        <div className="col-xl-6 col-lg-8 col-md-10 col-sm-12" style={{margin: 'auto'}}>
                            <div className="main-content-wrapper">
                                <Switch>
                                    {/*<Route exact path='/Admin/Users/' component={UserList}/>*/}
                                    {/*<Route exact path='/Admin/Users/:userID' component={UserCreate} />*/}
                                    {/*<Route exact path='/Admin/Locations/' component={LocationList}/>*/}
                                    {/*<Route exact path='/Admin/Locations/:locationID' component={LocationCreate} />*/}
                                    {/*<Route exact path='/Admin/Customers/' component={CustomerList}/>*/}
                                    {/*<Route exact path='/Admin/Customers/:customerID' component={CustomerCreate}/>*/}

                                    <Route exact path='/(.*)Locations/:locationID/Documents'
                                           component={(props) => (
                                               <ShowFiles timestamp={new Date().toString()} {...props} />
                                           )}/>
                                    <Route exact path='/(.*)Locations/:locationID/Documents/:folderID'
                                           component={(props) => (
                                               <ShowFiles timestamp={new Date().toString()} {...props} />
                                           )}/>
                                    <Route exact path='/Customers/:customerID/Documents'
                                           component={(props) => (
                                               <ShowFiles timestamp={new Date().toString()} {...props} />
                                           )}/>
                                    <Route exact path='/Customers/:customerID/Documents/:folderID'
                                           component={(props) => (
                                               <ShowFiles timestamp={new Date().toString()} {...props} />
                                           )}/>
                                    <Route exact path='/Customers/' component={ShowCustomers}/>
                                    {/*<Route exact path='/Locations/' component={Locations}/>*/}
                                    <Route exact path='/Customers/:customerID/' component={Locations}/>
                                    <Route exact path='/(.*)Locations/:locationID/' component={LocationView}/>
                                    <Route exact path='/(.*)Locations/:locationID/Users' component={Team}/>
                                    <Route exact path='/(.*)Locations/:locationID/Logs' component={Logs}/>
                                    <Route exact path='/(.*)Locations/:locationID/Users/:profileID'
                                           component={UserProfile}/>
                                         <Route exact path='/(.*)Locations/:locationID/Message/'
                                            component={MessageOffice}/>
                                    <Route exact path='/(.*)Locations/:locationID/CleaningPlans/'
                                           component={PlanTypes}/>
                                    <Route exact path='/(.*)Locations/:locationID/CleaningPlans/:type/Floors'
                                           component={CleaningPlan}/>
                                    <Route exact path='/(.*)Locations/:locationID/CleaningPlans/:type/Floors/:floor/'
                                           component={CleaningTask}/>
                                    <Route exact path='/(.*)Locations/:locationID/CleaningPlans/:type/:description/'
                                           component={CleaningTask}/>
                                    <Route exact
                                           path='/(.*)Locations/:locationID/CleaningPlans/:type/Floors/:floor/History/:taskID'
                                           component={CleaningHistory}/>
                                    <Route exact
                                           path='/(.*)Locations/:locationID/CleaningPlans/:type/:description/History/:taskID'
                                           component={CleaningHistory}/>
                                    <Route exact path='/(.*)Locations/:locationID/QualityReports'
                                           component={ListQualityReports}/>
                                    <Route exact path='/(.*)Locations/:locationID/QualityReports/:reportID/Floors'
                                           component={QualityReportFloors}/>
                                    <Route exact path='/(.*)Locations/:locationID/QualityReports/:reportID/Floors/CustomerComment/'
                                           component={CustomerComment}/>
                                    <Route exact
                                           path='/(.*)Locations/:locationID/QualityReports/:reportID/Floors/:floorID'
                                           component={QualityReport}/>
                                    <Route exact
                                           path='/(.*)Locations/User/:userID/:locationID/AddCleaningTask'
                                           component={AddCleaningTask}/>
                                    <Route exact path='/Rate/:ratingID' component={RateCollaboration}/>
                                    <Route exact path='/ChangePassword/' component={ChangePassword}/>
                                </Switch>
                            </div>
                        </div>
                    </div>
                </main>
                <Footer/>
            </div>
        );
    }
}

export default App;
