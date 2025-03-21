import { NavLink, useNavigate } from "react-router-dom";
import { Calendar, Layout, Menu, X, BookOpen, LogOut } from "lucide-react";
import { useState } from "react";
import logo from '../public/logo.webp'

const Navbar = ({ role }) => {
    const [isMobileMenuOpen, setIsMobileMenuOpen] = useState(false);
    const navigate = useNavigate()

    const handleLogout = () => {
        fetch('http://localhost:3000/api/user/logout', {
            method: 'GET',
            credentials: "include"
        })
        navigate('/dashboard')
    };

    return (
        <nav className="bg-black shadow-lg">
            <div className="max-w-full mx-auto px-8">
                <div className="flex justify-between h-16 items-center">
                    {/* Logo and Brand */}
                    <div className="flex items-center mb-2">
                        <NavLink to="/login" className="flex items-center space-x-3">
                            <img src={logo} className="w-21 h-23" />
                        </NavLink>
                    </div>

                    {
                        role == 'organization' &&
                        < div className="hidden md:flex items-center justify-center flex-1 space-x-8">
                            <NavLink
                                to="/timetable"
                                className={({ isActive }) =>
                                    `flex items-center space-x-2 relative group px-3 py-2 text-lg font-medium 
                                ${isActive ? 'text-white' : 'text-white/70'}
                                hover:text-white transition-all duration-300`
                                }
                            >
                                <Calendar className="h-5 w-5" />
                                <span>New TimeTable</span>
                                <span className="absolute bottom-0 left-0 w-full h-0.5 bg-white transform scale-x-0 group-hover:scale-x-100 transition-transform duration-300"></span>
                            </NavLink>

                            <NavLink
                                to="/dashboard"
                                className={({ isActive }) =>
                                    `flex items-center space-x-2 relative group px-3 py-2 text-lg font-medium 
                                ${isActive ? 'text-white' : 'text-white/70'}
                                hover:text-white transition-all duration-300`
                                }
                            >
                                <Layout className="h-5 w-5" />
                                <span>Dashboard</span>
                                <span className="absolute bottom-0 left-0 w-full h-0.5 bg-white transform scale-x-0 group-hover:scale-x-100 transition-transform duration-300"></span>
                            </NavLink>

                        </div>
                    }
                    {/* Logout Button - Right Side */}
                    <div className="hidden md:flex items-center">
                        <button
                            onClick={handleLogout}
                            className="flex items-center space-x-2 px-4 py-2 text-white/70 hover:text-white transition-all duration-300 cursor-pointer"
                        >
                            <LogOut className="h-5 w-5" />
                            <span className="font-medium">Logout</span>
                        </button>
                    </div>

                    {/* Mobile Menu Button */}
                    <div className="md:hidden flex items-center">
                        <button
                            onClick={() => setIsMobileMenuOpen(!isMobileMenuOpen)}
                            className="text-white hover:text-gray-300 focus:outline-none focus:ring-2 focus:ring-white"
                        >
                            {isMobileMenuOpen ? (
                                <X className="h-6 w-6" />
                            ) : (
                                <Menu className="h-6 w-6" />
                            )}
                        </button>
                    </div>
                </div>

                {/* Mobile Menu */}
                {isMobileMenuOpen && (
                    <div className="md:hidden">
                        <div className="px-2 pt-2 pb-3 space-y-1">
                            <NavLink
                                to="/timetable"
                                className={({ isActive }) =>
                                    `flex items-center space-x-2 px-3 py-2 text-base font-medium 
                                    ${isActive ? 'text-white' : 'text-white/70'}
                                    hover:text-white transition-all duration-300`
                                }
                                onClick={() => setIsMobileMenuOpen(false)}
                            >
                                <Calendar className="h-5 w-5" />
                                <span>New TimeTable</span>
                            </NavLink>
                            <NavLink
                                to="/dashboard"
                                className={({ isActive }) =>
                                    `flex items-center space-x-2 px-3 py-2 text-base font-medium 
                                    ${isActive ? 'text-white' : 'text-white/70'}
                                    hover:text-white transition-all duration-300`
                                }
                                onClick={() => setIsMobileMenuOpen(false)}
                            >
                                <Layout className="h-5 w-5" />
                                <span>Dashboard</span>
                            </NavLink>
                            <button
                                onClick={handleLogout}
                                className="flex w-full items-center space-x-2 px-3 py-2 text-base font-medium text-white/70 hover:text-white transition-all duration-300"
                            >
                                <LogOut className="h-5 w-5" />
                                <span>Logout</span>
                            </button>
                        </div>
                    </div>
                )}
            </div>
        </nav >
    );
};

export default Navbar;  