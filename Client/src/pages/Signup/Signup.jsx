import { useState } from 'react';
import { LogIn } from 'lucide-react';
import { motion } from 'framer-motion';
import ChooseRole from './ChooseRole';
import OriginalSignUp from './OriginalSignUp';
import Images from '../Login/Images'
import { Link } from 'react-router-dom'
import { Helmet } from "react-helmet-async";
import ToastProvider from '../../components/Toaster'

const Signup = () => {
    const [step, setStep] = useState('role');
    const [selectedType, setSelectedType] = useState(null);

    const transitionVariants = {
        hidden: { opacity: 0, y: 20 },
        visible: { opacity: 1, y: 0, transition: { duration: 0.5, ease: "easeInOut" } },
    };

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
                    {/* Login Button */}
                    <div className="absolute top-8 right-8">
                        <Link
                            to='/login'
                            className="flex items-center gap-2 px-6 py-2.5 text-white/70 hover:text-white border border-white/10 rounded-lg hover:border-white/20 transition-all duration-200 backdrop-blur-sm cursor-pointer"
                        >
                            <LogIn className="h-4 w-4" />
                            <span className="font-medium">Login</span>
                        </Link>
                    </div>

                    <motion.div
                        key={step}
                        initial="hidden"
                        animate="visible"
                        variants={transitionVariants}
                        className="max-w-md w-full mx-auto"
                    >
                        {step === 'role' ? (
                            <ChooseRole
                                setStep={setStep}
                                selectedType={selectedType}
                                setSelectedType={setSelectedType}
                            />
                        ) : (
                            <OriginalSignUp
                                setStep={setStep}
                                selectedType={selectedType}
                            />
                        )}
                    </motion.div>
                </div>
            </div>
        </>
    );
};

export default Signup;