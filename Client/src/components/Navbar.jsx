import { NavLink } from "react-router-dom";

const Navbar = () => {
    return (
        <div>
            <ul className="bg-black flex min-w-full justify-center gap-x-16 pt-4">
                <li className="relative group">
                    <NavLink
                        to="/timetable"
                        className={({ isActive }) =>
                            `block text-lg font-medium ${isActive ? 'text-white' : 'text-white/70'
                            } hover:text-white cursor-pointer transition-colors duration-300`
                        }
                    >
                        New TimeTable
                        <span className="absolute top-7 left-0 bottom-0 w-full h-[2px] bg-white transform scale-x-0 group-hover:scale-x-100 transition-transform duration-300 origin-center"></span>
                    </NavLink>
                </li>
                <li className="relative group">
                    <NavLink
                        to="/dashboard"
                        className={({ isActive }) =>
                            `block text-lg font-medium ${isActive ? 'text-white' : 'text-white/70'
                            } hover:text-white cursor-pointer transition-colors duration-300`
                        }
                    >
                        Dashboard
                        <span className="absolute top-7 left-0 bottom-0 w-full h-[2px] bg-white transform scale-x-0 group-hover:scale-x-100 transition-transform duration-300 origin-center"></span>
                    </NavLink>
                </li>
            </ul>
        </div>
    );
};

export default Navbar;