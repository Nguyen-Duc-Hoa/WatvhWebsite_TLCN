import React from 'react'
import {
    Route, Redirect
} from 'react-router-dom';

function PrivateRoute({ children, isAuthenticated, role, ...rest }) {
    return (
        <Route
            {...rest}
            render={(location) => (
                !isAuthenticated ? (
                    children
                ) : (
                    <Redirect
                        to={{
                            pathname: '/',
                            state: { from: location }
                        }}
                    />
                )
            )}
        />
    )
}

export default PrivateRoute
