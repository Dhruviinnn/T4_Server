import React from 'react';
import { Clock, Mail, ArrowRight } from 'lucide-react';
import { Helmet } from "react-helmet-async";
import { Link } from 'react-router-dom';

const WaitingApproval = () => {
  return (
    <>
      <Helmet>
        <title>Waiting Approval | Time Fourthe</title>
        <link rel="icon" type="image/png" href="/home-icon.png" />
      </Helmet>
      <div className="min-h-screen bg-black flex items-center justify-center p-4">
        <div className="max-w-md w-full space-y-8 bg-white/5 p-8 rounded-2xl backdrop-blur-sm border border-white/10">
          <div className="text-center">
            <div className="flex justify-center mb-6">
              <div className="p-4 rounded-full bg-white/10">
                <Clock className="h-12 w-12 text-white" />
              </div>
            </div>
            <h2 className="text-3xl font-bold text-white mb-4">
              Waiting for Approval
            </h2>
            <p className="text-gray-400 text-lg mb-8">
              Your account is pending approval from Time Fourthe administrators. Please check your email for further instructions.
            </p>
            
            <div className="flex items-center justify-center space-x-2 text-white/70 mb-8">
              <Mail className="h-5 w-5" />
              <span>Check your email inbox</span>
            </div>

            <div className="space-y-4">
              <p className="text-gray-500">
                Once approved, you'll receive an email with login instructions.
              </p>
              <Link 
                to="/login"
                className="inline-flex items-center text-white hover:text-gray-300 transition-colors"
              >
                Return to login
                <ArrowRight className="ml-2 h-4 w-4" />
              </Link>
            </div>
          </div>
        </div>
      </div>
    </>
  );
};

export default WaitingApproval;