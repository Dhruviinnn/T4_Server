import { motion } from 'framer-motion'
import { Mail, X, Send } from 'lucide-react'
import { useState } from 'react';
import {toast} from 'sonner'

const ResetPass = ({setShowForgotPassword}) => {
    const [forgotEmail, setForgotEmail] = useState('');

    const handleForgotPassword = (e) => {
        e.preventDefault();
        console.log('Forget');
        fetch('http://localhost:3000/api/user/forgot/mail',{
            method:'POST',
            headers:{
                'Content-Type':'application/json'
            },
            body:JSON.stringify({email:forgotEmail})
        })
        .then(res=>res.json())
        .then(({result})=>{
            if(result){
                setShowForgotPassword(false)
                toast.success(result);
            }
        })
    }
    return (

        <motion.div
            initial={{ x: 300, opacity: 0 }}
            animate={{ x: 0, opacity: 1 }}
            exit={{ x: 300, opacity: 0 }}
            transition={{ type: "spring", bounce: 0.3 }}
            className="fixed bottom-4 right-4 w-96 bg-white/5 backdrop-blur-md border border-white/10 rounded-xl shadow-2xl overflow-hidden glow-border z-20"
        >
            <div className="p-6">
                <div className="flex justify-between items-center mb-4">
                    <h3 className="text-xl font-semibold text-white flex items-center gap-2">
                        <Mail className="w-5 h-5" />
                        Reset Password
                    </h3>
                    <button
                        onClick={() => setShowForgotPassword(false)}
                        className="text-gray-400 hover:text-white transition-colors"
                    >
                        <X className="w-5 h-5" />
                    </button>
                </div>

                <form onSubmit={handleForgotPassword} className="space-y-4">
                    <div>
                        <label htmlFor="email" className="block text-sm font-medium text-gray-300 mb-1">
                            Email Address
                        </label>
                        <input
                            type="email"
                            id="email"
                            value={forgotEmail}
                            onChange={(e) => setForgotEmail(e.target.value)}
                            className="input-glow w-full px-4 py-3 rounded-xl bg-white/5 border border-white/10 text-white placeholder-gray-500 focus:outline-none focus:ring-2 focus:ring-white focus:border-transparent shadow-md"
                            placeholder="Enter your email"
                            required
                        />
                    </div>

                    <button
                        type="submit"
                        className="w-full bg-white text-black py-3 rounded-xl hover:bg-gray-100 transition-colors flex items-center justify-center group font-medium"
                    >
                        Send Reset Link
                        <Send className="ml-2 h-4 w-4 group-hover:translate-x-1 transition-transform" />
                    </button>
                </form>

                <p className="mt-4 text-sm text-gray-400">
                    We'll send you an email with instructions to reset your password.
                </p>
            </div>
        </motion.div>
    )
}

export default ResetPass
