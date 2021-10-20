import './App.less';
import { BrowserRouter as Router, Switch, Route } from 'react-router-dom'
import Home from './pages/Home/Home';
import ErrorBoundary from './components/ErrorBoundary/ErrorBoundary';
import Products from './pages/Products/Products';
import Product from './pages/Product/Product'
import Checkout from './pages/Checkout/Checkout'
import Payment from './pages/Payment/Payment';
import Login from './pages/Login/Login';
import Register from './pages/Register/Register';
import OrderHistory from './pages/OrderHistory/OrderHistory';
import OrderDetail from './pages/OrderDetail/OrderDetail';
import AdminMain from './pages/Admin/AdminMain/AdminMain';
import PublicRoute from './components/Routes/PublicRoute';
import UserLayout from './components/Layouts/UserLayout';
import Profile from './pages/Profile/Profile';
import PaymentSuccess from './pages/PaymentSuccess/PaymentSuccess';

function App() {
  return (
    <Router>
      <ErrorBoundary>
        <Switch>
          <PublicRoute
            path='/'
            exact={true}
            component={Home}
            layout={UserLayout}
          />
          <PublicRoute
            path='/products/:id'
            component={Product}
            layout={UserLayout}
          />
          <PublicRoute
            path='/products'
            component={Products}
            layout={UserLayout}
          />
          <PublicRoute
            path='/checkout/payment'
            component={Payment}
            layout={UserLayout}
          />
          <PublicRoute
            path='/checkout'
            component={Checkout}
            layout={UserLayout}
          />
          <PublicRoute
            path='/login'
            component={Login}
            layout={UserLayout}
          />
          <PublicRoute
            path='/register'
            component={Register}
            layout={UserLayout}
          />
          <PublicRoute
            path='/orderHistory'
            component={OrderHistory}
            layout={UserLayout}
          />
          <PublicRoute
            path='/orderDetail/:id'
            component={OrderDetail}
            layout={UserLayout}
          />
          <PublicRoute
            path='/paymentSuccess'
            component={PaymentSuccess}
            layout={UserLayout}
          />
          <PublicRoute
            path='/profile'
            component={Profile}
            layout={UserLayout}
          />
          <Route
            path='/admin'
            component={AdminMain}
          />
        </Switch>
      </ErrorBoundary>
    </Router>
  );
}

export default App;
