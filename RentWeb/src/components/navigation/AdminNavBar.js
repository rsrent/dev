import React from 'react';
import './AdminNavBar.css'
import {Link} from "react-router-dom";
export const AdminNavBar = (props) => {
  return <div className="nav-bar row">
          <Link className="col-2" to={{ pathname: `/Admin/Users/` }}>
            <div>
              <h5>Users</h5>
            </div>
          </Link>
          <Link className="col-2" to={{ pathname: `/Admin/Customers/` }}>
            <div>
              <h5>Customers</h5>
            </div>
          </Link>
          <Link className="col-2" to={{ pathname: `/Admin/Locations/` }}>
            <div>
              <h5>Locations</h5>
            </div>
          </Link>
        </div>
}
