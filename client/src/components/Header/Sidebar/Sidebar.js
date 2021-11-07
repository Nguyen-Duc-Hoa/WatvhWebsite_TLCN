import React from "react";
import "./Sidebar.scss";
import { connect } from "react-redux";
import { IoMdClose } from "react-icons/io";
import SidebarItem from "./SidebarItem/SidebarItem";
import SidebarAccount from "./SidebarAccount/SidebarAccount";
import * as actionTypes from "../../../store/actions/actionTypes";
import * as actions from "../../../store/actions/index";

const brands = [
  "Casio",
  "Amani",
  "BOSS",
  "Bulvova",
  "HUGO",
  "Citizen",
  "Tissot",
];
const menu = ["Home", "Brands", "Ladies", "Mens"];

function Sidebar({
  showSidebar,
  onCloseSidebar,
  onCloseOverlay,
  isAuth,
  onLogout,
}) {
  const closeSidebarHandler = () => {
    onCloseSidebar();
    onCloseOverlay();
  };

  return (
    <div className={`sidebar ${showSidebar && "active"}`}>
      <div className="sidebar__close" onClick={closeSidebarHandler}>
        <IoMdClose />
      </div>
      <ul className="sidebar__menu">
        {menu.map((ele, index) => (
          <SidebarItem
            key={index}
            content={ele}
            submenu={ele !== "Brands" ? brands : false}
          />
        ))}
      </ul>
      <SidebarAccount isAuth={isAuth} onLogout={onLogout} />
    </div>
  );
}

const mapStateToProps = (state) => {
  return {
    showSidebar: state.ui.showSidebar,
    isAuth: state.auth.token !== null,
  };
};

const mapDispatchToProps = (dispatch) => {
  return {
    onCloseSidebar: () => dispatch({ type: actionTypes.CLOSE_SIDEBAR }),
    onCloseOverlay: () => dispatch({ type: actionTypes.CLOSE_OVERLAY }),
    onLogout: () => dispatch(actions.logout()),
  };
};

export default connect(mapStateToProps, mapDispatchToProps)(Sidebar);
