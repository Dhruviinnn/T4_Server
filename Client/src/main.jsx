import { createRoot } from 'react-dom/client'
import './index.css'
import { BrowserRouter, Routes, Route } from "react-router-dom";
import { TimeTableForm, Login, Dashboard, Signup, WaitingApproval,ResetPass, NotFound } from './pages/index'
import { HelmetProvider } from "react-helmet-async";
import { UserProvider } from './contexts/user.context';
import ResetPassword from './pages/ResetPass/Resetpass';


createRoot(document.getElementById('root')).render(
    <HelmetProvider>
        <UserProvider>
            <BrowserRouter>
                <Routes>
                    <Route path="/timetable" element={<TimeTableForm />} />
                    <Route path="/login" element={<Login />} />
                    <Route path="/signup" element={<Signup />} />
                    <Route path="/signup/:url" element={<Signup />} />
                    <Route path="/dashboard" element={<Dashboard />} />
                    <Route path="/waiting-approval" element={<WaitingApproval />} />
                    <Route path="/resetpass" element={<ResetPassword />} />
                </Routes>
            </BrowserRouter>
        </UserProvider>
    </HelmetProvider>
)
