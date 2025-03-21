import { createRoot } from 'react-dom/client'
import './index.css'
import { BrowserRouter, Routes, Route } from "react-router-dom";
import { TimeTableForm, Login, Dashboard, Signup, WaitingApproval, ResetPass, NotFound, AfterTimetable } from './pages/index'
import { HelmetProvider } from "react-helmet-async";
import { UserProvider } from './contexts/user.context';
import { Auth, NoAuth } from './middleware';


createRoot(document.getElementById('root')).render(
    <HelmetProvider>
        <UserProvider>
            <BrowserRouter>
                <Routes>
                    <Route path="/timetable" element={<TimeTableForm />} />
                    <Route path="/aftertimetable" element={<AfterTimetable />} />
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
                    <Route path="/reset-password?id" element={<ResetPass />} />
                    <Route path="/:nothing" element={<NotFound />} />
                </Routes>
            </BrowserRouter>
        </UserProvider>
    </HelmetProvider>
)
