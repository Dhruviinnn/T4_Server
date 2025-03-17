import { useRef, useState, useEffect } from 'react';
import { ArrowRight, Lock, Unlock } from 'lucide-react';
import { motion } from 'framer-motion';
import Images from './Images'
import { Link } from 'react-router-dom'
import { Helmet } from "react-helmet-async";
import { useNavigate } from 'react-router-dom';
import { toast } from 'sonner'
import ToastProvider from '../../components/Toaster'

const Login = () => {
    const [email, setEmail] = useState("");
    const [passwordType, setPasswordType] = useState("password")
    const [password, setPassword] = useState("");
    const passwordRef = useRef();
    const navigate = useNavigate()

    const showPassword = (e) => {
        e.preventDefault();
        passwordRef.current.type = passwordRef.current.type === "password" ? "text" : "password"
        setPasswordType(passwordRef.current.type)
    };

    const transitionVariants = {
        hidden: { opacity: 0, y: 20 },
        visible: { opacity: 1, y: 0, transition: { duration: 0.5, ease: "easeInOut" } },
    };

    const handleSubmit = (e) => {
        e.preventDefault()
        const data = { email, password };
        fetch('http://localhost:3000/api/user/login', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        })
            .then(res => res.json())
            .then(({ error, message, redirectUrl }) => {
                if (error) {
                    toast.error(message)
                }
                else {
                    toast.success(message)
                    setTimeout(() => {
                        navigate(redirectUrl)
                    }, 500);
                }
            })
    }

    return (
        <>
            <Helmet>
                <title>Login | Time Fourthe</title>
                <link rel="icon" type="image/png" href="/home-icon.png" />
            </Helmet>
            <ToastProvider />
            <div className="min-h-screen bg-black flex">
                <div className="w-full lg:w-1/2 flex flex-col justify-center px-8 lg:px-12 xl:px-24">
                    <motion.div
                        initial="hidden"
                        animate="visible"
                        variants={transitionVariants}
                        className="max-w-md w-full mx-auto"
                    >
                        <h1 className="text-white text-4xl font-bold mb-2">
                            Welcome back
                        </h1>
                        <p className="text-gray-400 mb-8">
                            Sign in to continue
                        </p>

                        <form onSubmit={handleSubmit} className="space-y-4">
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
                                Sign in
                                <ArrowRight className="ml-2 h-4 w-4 group-hover:translate-x-1 transition-transform" />
                            </button>

                            <div className="text-center mt-4">
                                <button className="text-gray-400 hover:text-white transition-colors text-sm cursor-pointer">
                                    Forgot your password?
                                </button>
                            </div>

                            <div className="text-center mt-6">
                                <p className="text-gray-400">
                                    Don't have an account?{" "}
                                    <Link
                                        to="/signup"
                                        className="text-white hover:text-gray-200 transition-colors cursor-pointer font-medium"
                                    >
                                        Sign up
                                    </Link>
                                </p>
                            </div>
                        </form>
                    </motion.div>
                </div>

                <Images />
            </div>
        </>
    );
}

export default Login;