import React from 'react';
import './SearchBar.css'
import {Link} from "react-router-dom";
export const SearchBar = (props) => {

  function handleChange(event)
  {
    let name = event.target.name;
    let value = event.target.value;

    if(name === 'searchOption') { props.updateState({ searchOption: value }); }
    if(name === 'sortOption') { props.updateState({ sortOption: value }); }
    if(name === 'searchText') { props.updateState({ searchText: value }); }
    if(name === 'reversedSearch') { props.updateState({ sortReversed: !props.sortReversed }); }
  }

  return <div className="searchBar">
          <div className="search-div flex-container">
            <h5>Search options</h5>
            <select name="searchOption" onChange={handleChange}>
              { props.searchOptions.map(o => { return <option value={o}>{o}</option> }) }
            </select>
            <input name="searchText" type="text" onChange={handleChange}/>
          </div>

          <div className="search-div flex-container">
            <h5>Sort by:</h5>
            <select name="sortOption" onChange={handleChange}>
              { props.searchOptions.map(o => { return <option value={o}>{o}</option> }) }
            </select>
            <h5>Sort reversed:</h5>
            <input name="reversedSearch" type="checkbox" className="sort-reverse-checkbox" onChange={handleChange}/>
          </div>
        </div>
}
