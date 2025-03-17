import { createRoot } from 'react-dom/client'
import './index.css'
import { BrowserRouter, Routes, Route } from "react-router-dom";
import { TimeTableForm, Login, Dashboard, Signup } from './pages/index'
import { HelmetProvider } from "react-helmet-async";

createRoot(document.getElementById('root')).render(
    <HelmetProvider>
        <BrowserRouter>
            <Routes>
                <Route path="/timetable" element={<TimeTableForm />} />
                <Route path="/login" element={<Login />} />
                <Route path="/signup" element={<Signup />} />
                <Route path="/dashboard" element={<Dashboard />} />
            </Routes>
        </BrowserRouter>
    </HelmetProvider>
)
