import React, { useEffect } from "react";
import "./Cart.scss";
import { TransitionGroup, CSSTransition } from "react-transition-group";
import CartItem from "./CartItem/CartItem";
import { IoMdClose } from "react-icons/io";
import { Link } from "react-router-dom";
import { connect } from "react-redux";
import * as actionTypes from "../../../store/actions/actionTypes";
import { Button } from "antd";
import * as actions from "../../../store/actions/index";

const products = [
  {
    name: "Citizens Navihawk",
    price: 524,
    brand: "Citizen",
    amount: 10,
    number: 2,
    image:
      "https://d1rkccsb0jf1bk.cloudfront.net/products/99986640/main/large/jy8037-50e_high_res-1439896505-1302.jpg",
  },
  {
    name: "Citizen Navihawk",
    price: 524,
    brand: "Citizen",
    amount: 10,
    number: 2,
    image:
      "https://d1rkccsb0jf1bk.cloudfront.net/products/99986640/main/large/jy8037-50e_high_res-1439896505-1302.jpg",
  },
  {
    name: "Citizen Navihawk",
    price: 524,
    brand: "Citizen",
    amount: 10,
    number: 2,
    image:
      "https://d1rkccsb0jf1bk.cloudfront.net/products/99986640/main/large/jy8037-50e_high_res-1439896505-1302.jpg",
  },
  {
    name: "Citizen Navihawk",
    price: 524,
    brand: "Citizen",
    amount: 10,
    number: 2,
    image:
      "https://d1rkccsb0jf1bk.cloudfront.net/products/99986640/main/large/jy8037-50e_high_res-1439896505-1302.jpg",
  },
  {
    name: "Citizen Navihawk",
    price: 524,
    brand: "Citizen",
    amount: 10,
    number: 2,
    image:
      "https://d1rkccsb0jf1bk.cloudfront.net/products/99986640/main/large/jy8037-50e_high_res-1439896505-1302.jpg",
  },
  {
    name: "Citizen Navihawk",
    price: 524,
    brand: "Citizen",
    amount: 10,
    number: 2,
    image:
      "https://d1rkccsb0jf1bk.cloudfront.net/products/99986640/main/large/jy8037-50e_high_res-1439896505-1302.jpg",
  },
];

function Cart({
  showCart,
  onCloseCart,
  onCloseOverlay,
  onFetchCart,
  isAuth,
  token,
  idUser,
  cart,
  total,
  onUpdateCart,
}) {
  useEffect(() => {
    if (isAuth) {
      onFetchCart(idUser, token);
    }
  }, [isAuth]);

  const closeCartHandler = () => {
    onCloseOverlay();
    onCloseCart();
  };

  const changeCartNumberHandler = (id, value) => {
    onUpdateCart(id, value, idUser, token);
  };

  return (
    <div className={`sidebar__cart ${showCart && "active"}`}>
      <div className="cart__header">
        <div>Cart</div>
        <div className="cart__close" onClick={closeCartHandler}>
          <IoMdClose />
        </div>
      </div>
      <TransitionGroup component="div" className="cart__body">
        {cart.map(
          ({ Id, Image, BrandName, Amount, Price, Quantity, Name }, index) => (
            <CSSTransition key={index} classNames="fade" timeout={300}>
              <CartItem
                id={index}
                idProduct={Id}
                image={Image}
                name={Name}
                amount={Amount}
                number={Quantity}
                price={Price}
                brand={BrandName}
                changeCartNumberHandler={changeCartNumberHandler}
              />
            </CSSTransition>
          )
        )}
      </TransitionGroup>
      <div className="cart__footer">
        <div className="subtotal">
          <div>SUBTOTAL</div>
          <p>${total}</p>
        </div>
        <Link to="/checkout">
          <Button block type="primary" style={{ height: "45px" }}>
            Check out
          </Button>
        </Link>
      </div>
    </div>
  );
}

const mapStateToProps = (state) => {
  return {
    showCart: state.ui.showCart,
    loadingCart: state.cart.loading,
    isAuth: state.auth.token != null,
    token: state.auth.token,
    idUser: state.auth.id,
    cart: state.cart.cart,
    total: state.cart.total,
  };
};

const mapDispatchToProps = (dispatch) => {
  return {
    onCloseCart: () => dispatch({ type: actionTypes.CLOSE_CART }),
    onCloseOverlay: () => dispatch({ type: actionTypes.CLOSE_OVERLAY }),
    onFetchCart: (idUser, token) => dispatch(actions.fetchCart(idUser, token)),
    onUpdateCart: (productId, quantity, userId, token) =>
      dispatch(actions.updateCart(productId, quantity, userId, token)),
  };
};

export default connect(mapStateToProps, mapDispatchToProps)(Cart);
