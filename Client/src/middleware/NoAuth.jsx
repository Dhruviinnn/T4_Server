import React from 'react'
import {Navigate} from 'react-router-dom'
import Cookie from 'js-cookie'

const NoAuth = ({children}) => !Cookie.get('auth')?children :<Navigate to='/dashboard'/>

export default NoAuth
