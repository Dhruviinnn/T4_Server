import { createRoot } from 'react-dom/client'
import './index.css'
import { BrowserRouter, Routes, Route } from "react-router-dom";
import { TimeTableForm, Login, Dashboard, Signup, WaitingApproval,ResetPass, NotFound } from './pages/index'
import { HelmetProvider } from "react-helmet-async";
import { UserProvider } from './contexts/user.context';
<<<<<<< HEAD
import ResetPassword from './pages/ResetPass/Resetpass';
=======
import { Auth, NoAuth } from './middleware';
>>>>>>> 661bb7360dbe730154975f4dfc7ebca83183e45c


createRoot(document.getElementById('root')).render(
    <HelmetProvider>
        <UserProvider>
            <BrowserRouter>
                <Routes>
                    <Route path="/timetable" element={<TimeTableForm />} />
                    <Route path="/login" element={
                        <NoAuth>
                            <Login />
                        </NoAuth>
                    } />
                    <Route path="/signup" element={
                        <NoAuth>
                            <Signup />
                        </NoAuth>
                    } />
                    <Route path="/signup/:url" element={
                        <NoAuth>
                            <Signup />
                        </NoAuth>} />
                    <Route path="/dashboard" element={
                        <Auth>
                            <Dashboard />
                        </Auth>} />
                    <Route path="/waiting-approval" element={<WaitingApproval />} />
                    <Route path="/resetpass" element={<ResetPassword />} />
                </Routes>
            </BrowserRouter>
        </UserProvider>
    </HelmetProvider>
)
