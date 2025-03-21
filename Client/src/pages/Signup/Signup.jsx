import { useState, useEffect, useRef } from 'react';
import { LogIn, ArrowRight, Lock, Unlock, ChevronDown } from 'lucide-react';
import { motion } from 'framer-motion';
import Images from '../Login/Images'
import { Link } from 'react-router-dom'
import { Helmet } from "react-helmet-async";
import ToastProvider from '../../components/Toaster'
import { useParams, useNavigate } from 'react-router-dom';
import { decode } from 'js-base64'
import { useUser } from '../../contexts/user.context';

const Signup = () => {
    const { url } = useParams()
    const [, setUser] = useUser();
    const [role, setRole] = useState(null);
    const [email, setEmail] = useState("");
    const [name, setName] = useState("")
    const [currentClass, setCurrentClass] = useState("")
    const [grades, setGrades] = useState([])
    const [orgId, setOrgId] = useState("")
    const [isClassSelectOpen, setIsClassSelectOpen] = useState(false)
    const [passwordType, setPasswordType] = useState("password")
    const [orgType, setOrgType] = useState([])
    const [password, setPassword] = useState("");
    const passwordRef = useRef();
    const selectRef = useRef();
    const navigate = useNavigate()

    
    const classesType = ["Kindergarten", "Junior High", "Higher Secondary", "College"]

    useEffect(() => {
        if (url) {
            const decodedUrl = JSON.parse(decode(url))
            const { role, orgId } = decodedUrl;
            setRole(role);
            setOrgId(orgId)

            if (role == 'student') {
                fetch(`http://localhost:3000/api/get/org/classes?orgId=${orgId}`)
                .then(res=>res.json())
                .then(({orgClasses})=>{
                    setGrades(orgClasses)
                    setCurrentClass(orgClasses[0])
                })
            }
        }
        else {
            setRole('organization');
        }
    }, [])

    const showPassword = (e) => {
        e.preventDefault();
        passwordRef.current.type = passwordRef.current.type === "password" ? "text" : "password"
        setPasswordType(passwordRef.current.type)
    };


    const handleDetailsSubmit = (e) => {
        e.preventDefault();
        const data = {
            email,
            name,
            password,
            role: role,
        };
        data.role != 'organization' ? data.orgId = orgId : data.orgType = orgType;
        if (data.role == 'student') data.class = currentClass;
        console.log(data);

        fetch('http://localhost:3000/api/user/signup', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        })
            .then(res => res.json())
            .then(({ error, message, userData }) => {
                if (error) {
                    toast.error(message), {
                        duration: 4000,
                        style: { backgroundColor: "Red", color: "White", fontSize: "1rem" },
                    }
                }
                else {
                    setUser(userData)
                    if (role == 'organization') navigate('/waiting-approval')
                    else {
                        toast.loading('message')
                        setTimeout(() => {
                            navigate('/timetable')
                        }, 500);
                    }
                }
            })

    };
    const capitilization = () => {
        if (role)
            return role.charAt(0).toUpperCase() + role.slice(1)
    }

    return (
        <>
            <Helmet>
                <title>Signup | Time Fourthe</title>
                <link rel="icon" type="image/png" href="/home-icon.png" />
            </Helmet>

            <div className="min-h-screen bg-black flex">
                <ToastProvider />
                <Images />

                <div className="w-full lg:w-1/2 flex flex-col justify-center px-8 lg:px-12 xl:px-24">
                    <div className="absolute top-8 right-8">
                        <Link
                            to='/login'
                            className="flex items-center gap-2 px-6 py-2.5 text-white/70 hover:text-white border border-white/10 rounded-lg hover:border-white/20 transition-all duration-200 backdrop-blur-sm cursor-pointer"
                        >
                            <LogIn className="h-4 w-4" />
                            <span className="font-medium">Login</span>
                        </Link>
                    </div>

                    <div className="max-w-md w-full mx-auto">
                        <h1 className="text-white text-4xl font-bold">
                            Create account
                        </h1>
                        <p className="text-gray-400 my-2">
                            as <strong>{capitilization()}</strong>
                        </p>
                        <form onSubmit={handleDetailsSubmit} className="space-y-4">
                            <div>
                                <input
                                    name='email'
                                    type="text"
                                    value={email}
                                    onChange={(e) => setEmail(e.target.value)}
                                    placeholder="Email"
                                    className="w-full px-4 py-3 rounded-xl bg-white/5 border border-white/10 text-white placeholder-gray-500 focus:outline-none focus:ring-2 focus:ring-white focus:border-transparent transition-all"
                                />
                            </div>
                            <div>
                                <input
                                    name='name'
                                    type="text"
                                    value={name}
                                    onChange={(e) => setName(e.target.value)}
                                    placeholder="Name"
                                    className="w-full px-4 py-3 rounded-xl bg-white/5 border border-white/10 text-white placeholder-gray-500 focus:outline-none focus:ring-2 focus:ring-white focus:border-transparent transition-all"
                                />
                            </div>
                            {
                                role == 'student' &&
                                <div className="space-y-2">
                                    <label className="text-sm text-gray-400">Classes</label>
                                    <div className="relative mt-2" ref={selectRef}>
                                        <div
                                            onClick={() => setIsClassSelectOpen(!isClassSelectOpen)}
                                            className="w-full px-4 py-3 rounded-xl bg-white/5 border border-white/10 text-white cursor-pointer flex items-center justify-between hover:bg-white/10 hover:border-white/20 transition-all"
                                        >{currentClass}
                                            <ChevronDown className={`h-4 w-4 transition-transform ${isClassSelectOpen ? 'rotate-180' : ''}`} />
                                        </div>

                                        {isClassSelectOpen && (
                                            <div className="absolute w-full max-h-72 overflow-scroll mt-2 py-2 bg-[#0d0d0d] rounded-xl border border-white/10 shadow-xl z-10">
                                                {grades.map((className) => (
                                                    <div
                                                        key={className}
                                                        onClick={() => {
                                                            setCurrentClass(className)
                                                            setIsClassSelectOpen(!isClassSelectOpen)
                                                        }}
                                                        className={`px-4 py-2 flex items-center space-x-2 cursor-pointer transition-colors
                                            ${grades.includes(className)
                                                                ? 'bg-white/10 text-white'
                                                                : 'text-gray-400 hover:bg-white/5 hover:text-white'
                                                            }`}
                                                    >
                                                        <span className="text-sm">{className}</span>
                                                    </div>
                                                ))}
                                            </div>
                                        )}
                                    </div>
                                </div>
                            }
                            {role != 'organization' ?
                                <div>
                                    <input
                                        name='orgid'
                                        type="text"
                                        value={orgId}
                                        disabled
                                        placeholder="Organization Id"
                                        className="w-full px-4 py-3 rounded-xl bg-white/5 border border-white/10 text-white placeholder-gray-500 focus:outline-none focus:ring-2 focus:ring-white focus:border-transparent transition-all"
                                    />
                                </div>
                                :
                                <div className="space-y-2">
                                    <label className=" text-gray-400">Organization Contains</label>
                                    <div className="grid grid-cols-2 gap-3 mt-2">
                                        {classesType.map((type) => (
                                            <div
                                                key={type}
                                                onClick={() => setOrgType(orgType => {
                                                    const index = classesType.indexOf(type)
                                                    return !orgType.includes(index) ? [...orgType, index] : orgType.filter(ot => ot != index)
                                                })}
                                                className={`p-3 rounded-xl border cursor-pointer transition-all duration-200 flex items-center space-x-2
                                                ${orgType.includes(classesType.indexOf(type))
                                                        ? 'border-white/20 bg-white/10 text-white'
                                                        : 'border-white/10 bg-white/5 text-gray-400 hover:bg-white/10 hover:border-white/20'
                                                    }`}
                                            >
                                                <div className={`w-4 h-4 rounded border flex items-center justify-center transition-colors
                                              ${orgType.includes(classesType.indexOf(type))
                                                        ? 'border-white bg-white'
                                                        : 'border-gray-500'
                                                    }`}
                                                >
                                                    {orgType.includes(classesType.indexOf(type)) && (
                                                        <div className="w-2 h-2 bg-black rounded-sm" />
                                                    )}
                                                </div>
                                                <span className="text-sm">{type}</span>
                                            </div>
                                        ))}
                                    </div>
                                </div>
                            }
                            <div>
                                <div className="relative">
                                    <input
                                        name='password'
                                        ref={passwordRef}
                                        type="password"
                                        value={password}
                                        onChange={(e) => setPassword(e.target.value)}
                                        placeholder="Password"
                                        className="w-full px-4 py-3 rounded-xl bg-white/5 border border-white/10 text-white placeholder-gray-500 focus:outline-none focus:ring-2 focus:ring-white focus:border-transparent transition-all"
                                    />
                                    {
                                        passwordType === "password" ?
                                            <Lock className="absolute right-3 top-1/2 transform -translate-y-1/2 h-5 w-5 text-gray-500 cursor-pointer hover:text-white transition-colors" onClick={showPassword} />
                                            :
                                            <Unlock className="absolute right-3 top-1/2 transform -translate-y-1/2 h-5 w-5 text-gray-500 cursor-pointer hover:text-white transition-colors" onClick={showPassword} />
                                    }
                                </div>
                            </div>

                            <button
                                type="submit"
                                className="w-full bg-white text-black py-3 rounded-xl hover:bg-gray-100 transition-colors flex items-center justify-center group cursor-pointer font-medium"
                            >
                                Create account
                                <ArrowRight className="ml-2 h-4 w-4 group-hover:translate-x-1 transition-transform" />
                            </button>

                            <div className="text-center mt-6">
                                <p className="text-gray-400">
                                    Already have an account?{" "}
                                    <a
                                        href="/login"
                                        className="text-white hover:text-gray-200 transition-colors cursor-pointer font-medium"
                                    >
                                        Log in
                                    </a>
                                </p>
                            </div>
                        </form>
                    </div>
                </div >
            </div >
        </>
    );
};

export default Signup;