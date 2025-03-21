import React from 'react';
import { Lock, ArrowRight } from 'lucide-react';
import { Helmet } from "react-helmet-async";
import { Link } from 'react-router-dom';

const ResetPassword = () => {
  return (
    <>
      <Helmet>
        <title>Reset Password | Time Fourthe</title>
        <link rel="icon" type="image/png" href="/home-icon.png" />
      </Helmet>
      <div className="min-h-screen bg-black flex items-center justify-center p-4 relative overflow-hidden">
        <div className="absolute inset-0 bg-black opacity-50 blur-2xl" />
        <div className="max-w-[40vw] w-full space-y-8 bg-white/5 p-8 rounded-2xl backdrop-blur-md border border-white/10 shadow-md relative z-10 transition-all duration-300 glow-border hover:shadow-white/20">
         
          <div className="text-center">
            <div className="flex justify-center mb-6 animate-pulse">
              <div className="p-4 rounded-full bg-white/10 border border-white/10 shadow-md">
                <Lock className="h-12 w-12 text-white" />
              </div>
            </div>
            <h2 className="text-3xl font-bold text-white mb-4 tracking-wide drop-shadow-md ">
              Reset Password
            </h2>
            <form className="space-y-6">
           
              <div>
                <label className="block text-gray-400 text-sm mb-2 ">New Password</label>
                <input 
                  type="password" 
                  className="w-full p-3 rounded-lg bg-white/10 text-white placeholder-gray-400 border border-white/10 focus:outline-none max-w-sm focus:ring-2 focus:ring-white/20"
                  placeholder="••••••••"
                />
              </div>
              <div>
                <label className="block text-gray-400 text-sm mb-2">Confirm New Password</label>
                <input 
                  type="password" 
                  className="w-full p-3 rounded-lg bg-white/10 text-white placeholder-gray-400 border border-white/10 focus:outline-none max-w-sm focus:ring-2 focus:ring-white/20"
                  placeholder="••••••••"
                />
              </div>
              <button 
                type="submit" 
                className="w-full bg-white text-black  mt-3 py-3 rounded-xl hover:bg-gray-100 transition-colors  justify-center group cursor-pointer font-medium max-w-sm"
              >
                UPDATE PASSWORD
              </button>
            </form>
            <div className="mt-6">
              <Link 
                to="/login"
                className="inline-flex items-center text-white hover:text-gray-300 transition-all duration-300 group"
              >
                Return to login
                <ArrowRight className="ml-2 h-4 w-4 group-hover:translate-x-1 transition-transform duration-300" />
              </Link>
            </div>
          </div>
        </div>
      </div>
    </>
  );
};

export default ResetPassword;